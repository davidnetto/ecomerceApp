using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A descrição do produto é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O preço do produto é obrigatório")]
        [Range(0.01, 99999.99, ErrorMessage = "O preço deve estar entre R$ 0,01 e R$ 99.999,99")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [Range(0, 10000, ErrorMessage = "A quantidade em estoque deve estar entre 0 e 10.000")]
        public int StockQuantity { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public bool IsAvailable => StockQuantity > 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
