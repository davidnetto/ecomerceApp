using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres")]
        public string FullName { get; set; }

        [PersonalData]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres")]
        public string Address { get; set; }

        [PersonalData]
        [StringLength(50, ErrorMessage = "A cidade deve ter no máximo 50 caracteres")]
        public string City { get; set; }

        [PersonalData]
        [StringLength(9, ErrorMessage = "O CEP deve ter no máximo 9 caracteres")]
        public string ZipCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
    }
}
