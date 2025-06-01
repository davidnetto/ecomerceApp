using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome da categoria deve ter no máximo 50 caracteres")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "A descrição da categoria deve ter no máximo 200 caracteres")]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
