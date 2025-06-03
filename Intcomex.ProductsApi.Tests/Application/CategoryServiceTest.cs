using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Application.Services;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Domain.Interfaces;
using Moq;
public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepository;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepository = new Mock<ICategoryRepository>();
        _service = new CategoryService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedCategoryDto()
    {
        // Arrange
        var inputDto = new CategoryDto
        {
            CategoryName = "TestCategory",
            Description = "Test description"
        };

        var createdEntity = new Category
        {
            CategoryId = 1,
            CategoryName = "TestCategory",
            Description = "Test description"
        };

        _mockRepository
            .Setup(r => r.CreateAsync(It.IsAny<Category>()))
            .ReturnsAsync(createdEntity);

        // Act
        var result = await _service.CreateAsync(inputDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdEntity.CategoryId, result.CategoryId);
        Assert.Equal(createdEntity.CategoryName, result.CategoryName);
        Assert.Equal(createdEntity.Description, result.Description);

        _mockRepository.Verify(r => r.CreateAsync(It.Is<Category>(c =>
            c.CategoryName == inputDto.CategoryName &&
            c.Description == inputDto.Description)), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenFound()
    {
        // Arrange
        var categoryId = 1;
        var categoryEntity = new Category
        {
            CategoryId = categoryId,
            CategoryName = "Category1",
            Description = "Description1"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(categoryId))
                       .ReturnsAsync(categoryEntity);

        // Act
        var result = await _service.GetByIdAsync(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryEntity.CategoryId, result?.CategoryId);
        Assert.Equal(categoryEntity.CategoryName, result?.CategoryName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var categoryId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(categoryId))
                       .ReturnsAsync((Category?)null);

        // Act
        var result = await _service.GetByIdAsync(categoryId);

        // Assert
        Assert.Null(result);
    }
}
