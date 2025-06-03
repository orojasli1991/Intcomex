using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Intcomex.ProductsApi.Application.Services;
using Intcomex.ProductsApi.Domain.Interfaces;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Application.Dto;
using System.Linq;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProductDto_WhenProductExists()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 1,
            ProductName = "Test",
            UnitPrice = 100,
            CategoryId = 2,
            Category = new Category { Picture = new byte[] { 1, 2, 3 } }
        };
        _mockRepo.Setup(r => r.GetProducts(1)).ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
        Assert.Equal("Test", result.ProductName);
    }
    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenProductExists()
    {
        var product = new Product { ProductId = 1, ProductName = "Old", UnitPrice = 10, CategoryId = 1 };
        var dto = new ProductDto { ProductId = 1, ProductName = "New", UnitPrice = 50, CategoryId = 2 };

        _mockRepo.Setup(r => r.GetProducts(1)).ReturnsAsync(product);
        _mockRepo.Setup(r => r.UpdateProduct(1, It.IsAny<Product>())).Returns(Task.CompletedTask);

        var result = await _service.UpdateAsync(dto);

        Assert.True(result);
    }
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenProductExists()
    {
        var product = new Product { ProductId = 1 };
        _mockRepo.Setup(r => r.GetProducts(1)).ReturnsAsync(product);
        _mockRepo.Setup(r => r.DeleteProduct(1)).Returns(Task.CompletedTask);

        var result = await _service.DeleteAsync(1);

        Assert.True(result);
    }

}
