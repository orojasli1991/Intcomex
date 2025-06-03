using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Application.Models;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly Random _random = new();


        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<PagedResult<ProductDto>> GetAllAsync(int pageNumber,int pageSize, int? categoryId,string? search)
        {
            var (products, totalCount) = await _repository.GetProducts(pageNumber, pageSize, categoryId, search);
            var productDtos = products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CategoryId = p.CategoryId
            });
            return new PagedResult<ProductDto>
            {
                Items = productDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _repository.GetProducts(id);
            if (p == null) return null;

            return new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CategoryId = p.CategoryId,
                CategoryPicture = p.Category.Picture != null ? Convert.ToBase64String(p.Category.Picture): null
                
            };
        }
        public async Task CreateAsync(int totalCount)
        {
            if (totalCount <= 0)
                throw new ArgumentException("Debe generar al menos un producto.");

            int maxBatchSize = 10000;
            int batchSize = Math.Min(maxBatchSize, Math.Max(1000, totalCount / 10));

            for (int i = 0; i < totalCount; i += batchSize)
            {
                var batch = new List<ProductDto>();
                int currentBatchSize = Math.Min(batchSize, totalCount - i);

                for (int j = 0; j < currentBatchSize; j++)
                {
                    batch.Add(await GenerateRandomProduct());
                }
                var batchEntities = batch.Select(dto => new Product
                {
                    ProductName = dto.ProductName,
                    CategoryId = dto.CategoryId,
                    SupplierId = dto.SupplierId,
                    QuantityPerUnit = dto.QuantityPerUnit,
                    UnitPrice = dto.UnitPrice,
                    UnitsInStock = dto.UnitsInStock,
                    UnitsOnOrder = dto.UnitsOnOrder,
                    ReorderLevel = dto.ReorderLevel,
                    Discontinued = dto.Discontinued
                }).ToList();
                await _repository.CreateProduct(batchEntities);
            }

        }
        public async Task<bool> UpdateAsync(ProductDto dto)
        {
            var existing = await _repository.GetProducts(dto.ProductId);
            if (existing == null) return false;

            existing.ProductName = dto.ProductName;
            existing.UnitPrice = dto.UnitPrice;
            existing.CategoryId = dto.CategoryId;

            await _repository.UpdateProduct(existing.ProductId, existing);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetProducts(id);
            if (existing == null) return false;

            await _repository.DeleteProduct(existing.ProductId);
            return true;
        }
    private async Task<ProductDto> GenerateRandomProduct()
        {
     
            return new ProductDto
            {
                ProductName = "Product " + Guid.NewGuid().ToString("N"),
                CategoryId = await GetRandomCategoryIdAsync(),
                //CategoryId = _random.Next(5, 6), // toca ir a preguntar q id de categorias existen  
                SupplierId = _random.Next(1, 4),
                QuantityPerUnit = $"{_random.Next(1, 100)} unidades",
                UnitPrice = (decimal)(_random.NextDouble() * 100),
                UnitsInStock = (short)_random.Next(0, 100),
                UnitsOnOrder = (short)_random.Next(0, 100),
                ReorderLevel = (short)_random.Next(1, 10),
                Discontinued = _random.Next(0, 2) == 0

            };
        }
        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://localhost:44350/Category"); // Cambia la URL si es distinta

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<int> GetRandomCategoryIdAsync()
        {
            var categories = await GetAllCategoriesAsync();

            if (categories == null || categories.Count == 0)
                throw new Exception("No hay categorías disponibles.");

            var random = new Random();
            var randomCategory = categories[random.Next(categories.Count)];
            return randomCategory.CategoryId;
        }
    }
}