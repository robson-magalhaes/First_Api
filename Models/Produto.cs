using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UniApi.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProdutoNome { get; set; }

        public string? Descricao { get; set; }

        public int CategoriaId { get; set; }
        public virtual Categoria? Categorias { get; set; }
    }
}
