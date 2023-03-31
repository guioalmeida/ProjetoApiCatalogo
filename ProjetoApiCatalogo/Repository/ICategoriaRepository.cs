using ProjetoApiCatalogo.Models;
using ProjetoApiCatalogo.Pagination;

namespace ProjetoApiCatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria> 
    {
        PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
