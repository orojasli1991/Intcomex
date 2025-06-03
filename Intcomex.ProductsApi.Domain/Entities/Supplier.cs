using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Domain.Entities
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required]
        [MaxLength(30)]
        public string CompanyName { get; set; } = null!;
        [MaxLength(30)]
        public string? ContactName { get; set; }
        [MaxLength(30)]
        public string? ContactTitle { get; set; }
        [MaxLength(30)]
        public string? Address { get; set; }
        [MaxLength(30)]
        public string? City { get; set; }
        [MaxLength(30)]
        public string? Region { get; set; }
        [MaxLength(30)]
        public string? PostalCode { get; set; }
        [MaxLength(30)]
        public string? Country { get; set; }
        [MaxLength(10)]
        public string? Phone { get; set; }
        [MaxLength(30)]
        public string? Fax { get; set; }
        public string? HomePage { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
