using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoPets.Api.Models
{
    public class ProductAddDto
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(minimum: 0.01, maximum: (double)decimal.MaxValue)]
        public decimal Price { get; set; }
    }
}
