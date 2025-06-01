using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models.Entities
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CartId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "A quantidade deve estar entre 1 e 100")]
        public int Quantity { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }
    }
}
