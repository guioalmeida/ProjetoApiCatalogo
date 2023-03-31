using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoApiCatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage ="O nome é obrigatório")]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName ="decimal(8,2")]
        [Range(1,10000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }

        [Required]
        [MaxLength(300)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        public Categoria? Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
