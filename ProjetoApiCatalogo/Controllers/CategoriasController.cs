
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
    [Authorize(AuthenticationSchemes ="Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork contexto, IMapper mapper)
        {
            _context = contexto;
            _mapper = mapper;   
        }

        [HttpGet("produtos")]

        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = _context.CategoriaRepository.GetCategoriasProdutos().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }

        }

        [HttpGet]

        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriaParameters categoriaParameters)
        {
            try 
            {
                var categorias = _context.CategoriaRepository.GetCategorias(categoriaParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound($"A categoria com o id={id} não foi encontrada");
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Add(categoria);
                _context.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar uma nova categoria");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível alterar a categoria com o id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Update(categoria);
                _context.Commit();

                return Ok($"A categoria com o id={id} foi atualizada com sucesso");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar a categoria com o id={id}");
            }

        }

            [HttpDelete("{id}")]
            public ActionResult<CategoriaDTO> Delete(int id)
            {
            try
            {
                var categoria = _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com o id={id} não foi encontrada");
                }
                _context.CategoriaRepository.Delete(categoria);
                _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao excluir a categoria com o id={id}");
            }
                
            }
        }
    }

