using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoApiCatalogo.Context;
using ProjetoApiCatalogo.DTOs;
using ProjetoApiCatalogo.Models;
using ProjetoApiCatalogo.Pagination;
using ProjetoApiCatalogo.Repository;

namespace ProjetoApiCatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoPreco()
        {
            var produtos = _uof.ProdutoRepository.GetProdutoPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDto;
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p=> p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDto)
        {
           
            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id,[FromBody] ProdutoDTO produtoDto) 
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            {
                if (produto == null)
                    return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
    }
}
