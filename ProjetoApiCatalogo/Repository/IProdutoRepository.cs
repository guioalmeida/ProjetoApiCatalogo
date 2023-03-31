using ProjetoApiCatalogo.Models;
using ProjetoApiCatalogo.Pagination;

namespace ProjetoApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutoPorPreco();
    }
}
