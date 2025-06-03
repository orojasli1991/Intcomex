using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Domain.Entities
{
    public class Shipper
    {
        [Key]
        public int ShipperId { get; set; }
        [Required]
        [MaxLength(30)]
        public string CompanyName { get; set; } = null!;
        [MaxLength(10)]
        public string? Phone { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
