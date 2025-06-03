using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Domain.Interfaces;
using Intcomex.ProductsApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(AppDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al crear la categoría.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocurrió un error inesperado al crear la categoría.", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al obtener las categorías desde la base de datos.", ex);
                throw;
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categories.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener la categoría con ID {id}.", ex);
                throw;
            }
        }
    }
}
