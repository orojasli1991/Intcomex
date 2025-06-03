using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = null!;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int SupplierId { get; set; } 

        [MaxLength(50)]
        public string? QuantityPerUnit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!;
        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
