using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Domain.Interfaces;
using Intcomex.ProductsApi.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public ProductRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProducts(int pageNumber, int pageSize, int? categoryId, string? search)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                if (categoryId.HasValue)
                    query = query.Where(p => p.CategoryId == categoryId.Value);

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(p => p.ProductName.Contains(search));

                var totalCount = await query.CountAsync();

                var products = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (products, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de productos.", ex);
            }
        }

        public async Task<Product> GetProducts(int id)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p=> p.ProductId== id)
                    ?? throw new Exception($"Producto con ID {id} no encontrado.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el producto con ID {id}.", ex);
            }
        }

        public async Task CreateProduct(IEnumerable<Product> products)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var bulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = "Products"
                };

                var table = new DataTable();
                table.Columns.Add("ProductId", typeof(int));
                table.Columns.Add("ProductName", typeof(string));
                table.Columns.Add("CategoryId", typeof(int));
                table.Columns.Add("SupplierId", typeof(int));
                table.Columns.Add("QuantityPerUnit", typeof(string));
                table.Columns.Add("UnitPrice", typeof(decimal));
                table.Columns.Add("UnitsInStock", typeof(short));
                table.Columns.Add("UnitsOnOrder", typeof(short));
                table.Columns.Add("ReorderLevel", typeof(short));
                table.Columns.Add("Discontinued", typeof(bool));

                foreach (var p in products)
                {
                    table.Rows.Add(p.ProductId, p.ProductName, p.CategoryId, p.SupplierId, p.QuantityPerUnit,
                                   p.UnitPrice, p.UnitsInStock, p.UnitsOnOrder, p.ReorderLevel, p.Discontinued);
                }

                await connection.OpenAsync();
                await bulkCopy.WriteToServerAsync(table);
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al realizar la carga masiva de productos.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error inesperado durante la creación de productos.", ex);
            }
        }

        public async Task UpdateProduct(int id, Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error al actualizar el producto con ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al actualizar el producto con ID {id}.", ex);
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Producto con ID {id} no encontrado para eliminación.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error al eliminar el producto con ID {id}.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al eliminar el producto con ID {id}.", ex);
            }
        }
    }
}
