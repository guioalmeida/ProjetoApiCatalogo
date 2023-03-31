using Microsoft.EntityFrameworkCore;
using ProjetoApiCatalogo.Context;
using ProjetoApiCatalogo.Models;
using ProjetoApiCatalogo.Pagination;

namespace ProjetoApiCatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters) 
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome),
                                                                  categoriaParameters.PageNumber,
                                                                  categoriaParameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
