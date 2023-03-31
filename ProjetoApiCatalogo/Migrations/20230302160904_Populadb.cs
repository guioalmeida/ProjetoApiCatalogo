using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class Populadb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome,ImagemUrl) Values('Bebidas'," +
                "'https://www.macoratti.net/Imagens/1.jpg')");
            mb.Sql("Insert into Categorias(Nome,ImagemUrl) Values('Lanches'," +
                "'https://www.macoratti.net/Imagens/2.jpg')");
            mb.Sql("Insert into Categorias(Nome,ImagemUrl) Values('Sobremesas'," +
                "'https://www.macoratti.net/Imagens/3.jpg')");

            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque," +
                "DataCadastro,CategoriaId) Values('Coca-Cola Diet','Refrigerante de Cola 350ml'," +
                "5.45,'https://macoratti.net/Imagens/coca-cola.jpg',50,now()," +
                "(Select CategoriaId from Categorias where Nome='Bebidas'))");

            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque," +
               "DataCadastro,CategoriaId) Values('Lanche de Atum','Lanche de Atum com Maionese'," +
               "8.45,'https://macoratti.net/Imagens/lanchedeatum.jpg',10,now()," +
               "(Select CategoriaId from Categorias where Nome='Lanches'))");

            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque," +
               "DataCadastro,CategoriaId) Values('Pudim de Leite','Pudim de leite condensado'," +
               "6.45,'https://macoratti.net/Imagens/pudim.jpg',20,now()," +
               "(Select CategoriaId from Categorias where Nome='Sobremesas'))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
