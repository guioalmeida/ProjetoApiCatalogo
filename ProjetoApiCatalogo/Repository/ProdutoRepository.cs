﻿using ProjetoApiCatalogo.Context;
using ProjetoApiCatalogo.Models;
using ProjetoApiCatalogo.Pagination;

namespace ProjetoApiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
            
        }

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            //    .Take(produtosParameters.PageSize)
            //    .ToList();

            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId), produtosParameters.PageNumber,
                produtosParameters.PageSize);
        }
        public IEnumerable<Produto> GetProdutoPorPreco()
        {
            return Get().OrderBy(c=>c.Preco).ToList();
        }
    }
}
