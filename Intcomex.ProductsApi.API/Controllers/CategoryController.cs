using AutoMapper;
using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intcomex.ProductsApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryServices;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryServices, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryDto inputDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido al intentar crear categoría: {@InputDto}", inputDto);
                return BadRequest(ModelState);
            }

            try
            {
                var categoryDto = await _categoryServices.CreateAsync(inputDto);
                _logger.LogInformation("Categoría creada exitosamente con ID {CategoryId}", categoryDto.CategoryId);
                return CreatedAtAction(nameof(GetById), new { id = categoryDto.CategoryId }, categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoría.");
                return StatusCode(500, "Ha ocurrido un error al crear la categoría.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _categoryServices.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("No se encontró categoría con ID {CategoryId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Categoría consultada con ID {CategoryId}", id);
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la categoría con ID {CategoryId}", id);
                return StatusCode(500, "Error interno al consultar la categoría.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Categorys()
        {
            try
            {
                var categories = await _categoryServices.GetAllAsync();
                _logger.LogInformation("Se obtuvieron {Count} categorías", categories.Count());
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías.");
                return StatusCode(500, "Error al obtener las categorías.");
            }
        }
    }
}
