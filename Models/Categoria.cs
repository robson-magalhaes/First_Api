using System.ComponentModel.DataAnnotations;

namespace UniApi.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string? CategoriaNome { get; set; }

        public IEnumerable<Produto>? Produtos { get; set; }
    }
}
