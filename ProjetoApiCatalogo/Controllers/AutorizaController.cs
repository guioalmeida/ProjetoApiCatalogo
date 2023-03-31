using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetoApiCatalogo.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, 
                SignInManager<IdentityUser> signInManager,
                IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em :: "
                + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            // O [ApiController] faz isso automaticamente

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            //}

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok (GeraToken(model));
        }

        [HttpPost("login")]
        
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(GeraToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "login inválido, tente novamnete!");
                return BadRequest(ModelState);
            }
        }

        private UsuarioToken GeraToken(UsuarioDTO userInfo)
        {
            //Isso não é obg, fiz para tornar mais seguro o token
            //define declarações do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("meuPet", "bob"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //gera uma chave com base em um algoritmo simetrico
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            //gera a assinatura digital do token usando o algoritmo Hmac e a chave privada 
            var credencias = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tempo de expiração do token 
            var expiracao = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            //Classe que representa um token JWT e gera o token 
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credencias);

            //retorna os dados com o token e informações 
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT Ok"
            };
        }
    }
}
