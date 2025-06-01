using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pendente";

        // Informações de envio
        [Required]
        [StringLength(100)]
        public string ShippingName { get; set; }

        [Required]
        [StringLength(200)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string ShippingCity { get; set; }

        [Required]
        [StringLength(9)]
        public string ShippingZipCode { get; set; }

        // Informações de pagamento
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        // Relacionamentos
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
