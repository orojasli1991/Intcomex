using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intcomex.ProductsApi.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductService productServices, ILogger<ProductsController> logger)
        {
            _service = productServices;
            _logger = logger;
        }

        #region GET
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? search = null)
        {
            try
            {
                var products = await _service.GetAllAsync(pageNumber,pageSize,categoryId,search);
                _logger.LogInformation("Se obtuvieron {Count} productos", products?.Items?.Count() ?? 0);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los productos.");
                return StatusCode(500, "Error al obtener los productos.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProducts(int id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Producto no encontrado con ID {ProductId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Producto obtenido con ID {ProductId}", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {ProductId}.", id);
                return StatusCode(500, $"Error al obtener el producto con ID {id}.");
            }
        }
        #endregion

        #region POST
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> CreateProduct(int inputCount)
        {
            try
            {
                await _service.CreateAsync(inputCount);
                _logger.LogInformation("{InputCount} productos aleatorios generados e insertados exitosamente.", inputCount);
                return Ok($"{inputCount} productos aleatorios generados e insertados exitosamente.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de argumento al generar productos.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar los productos.");
                return StatusCode(500, "Error al generar los productos.");
            }
        }
        #endregion

        #region PUT
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto product)
        {
            if (id != product.ProductId)
            {
                _logger.LogWarning("Intento de actualización con IDs inconsistentes: {IdParam} != {ProductId}", id, product.ProductId);
                return BadRequest("El ID del producto no coincide.");
            }

            try
            {
                var updated = await _service.UpdateAsync(product);
                if (!updated)
                {
                    _logger.LogWarning("No se encontró producto para actualizar con ID {ProductId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Producto actualizado con ID {ProductId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto con ID {ProductId}.", id);
                return StatusCode(500, $"Error al actualizar el producto con ID {id}.");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("No se encontró producto para eliminar con ID {ProductId}", id);
                    return NotFound();
                }

                _logger.LogInformation("Producto eliminado con ID {ProductId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto con ID {ProductId}.", id);
                return StatusCode(500, $"Error al eliminar el producto con ID {id}.");
            }
        }
        #endregion
    }
}
