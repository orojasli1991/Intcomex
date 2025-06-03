using Intcomex.ProductsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Intcomex.ProductsApi.Domain.Interfaces
{
   public interface ICategoryRepository
    {
        public  Task<Category> CreateAsync(Category category);
        public  Task<IEnumerable<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(int id);
    }
}
