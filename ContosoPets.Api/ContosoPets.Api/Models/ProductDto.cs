using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoPets.Api.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(minimum: 0.01, maximum: (double)decimal.MaxValue)]
        public decimal Price { get; set; }
    }
}
