using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto inputcategory)
        {
            var category = new Category
            {
                CategoryName = inputcategory.CategoryName,
                Description = inputcategory.Description,
                
            };
            if (inputcategory.Picture != null && inputcategory.Picture.Length > 0)
            {
                using var ms = new MemoryStream();
                await inputcategory.Picture.CopyToAsync(ms);
                category.Picture = ms.ToArray();
            }
            var created = await _repository.CreateAsync(category);

            return new CategoryDto
            {
                CategoryId = created.CategoryId,
                CategoryName = created.CategoryName,
                Description = created.Description

            };
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return null;
            string? pictureUrl = null;
            if (category.Picture != null && category.Picture.Length > 0)
            {
                var base64 = Convert.ToBase64String(category.Picture);
                pictureUrl = $"data:image/jpeg;base64,{base64}"; // Cambia image/jpeg si usas PNG u otro formato
            }
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                PictureUrl = pictureUrl
            };
        }
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categorys = await _repository.GetAllAsync();

            return categorys.Select(p => new CategoryDto
            {
                CategoryId= p.CategoryId,
                CategoryName = p.CategoryName,
                Description = p.Description,
                PictureUrl = p.Picture != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(p.Picture)}": null

            }); ;
        }

    }
}
