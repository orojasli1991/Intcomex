using Intcomex.ProductsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Domain.Interfaces
{
   public interface IProductRepository
    {
        public Task<(IEnumerable<Product> Products, int TotalCount)> GetProducts(int pageNumber, int pageSize, int? categoryId, string? search);
        public Task<Product> GetProducts(int id);
        public Task CreateProduct(IEnumerable<Product> products);
        public Task UpdateProduct(int id, Product product);
        public Task DeleteProduct(int id);
    }
}
