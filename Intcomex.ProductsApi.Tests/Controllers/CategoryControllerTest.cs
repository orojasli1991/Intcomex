using AutoMapper;
using Intcomex.ProductsApi.API.Controllers;
using Intcomex.ProductsApi.Application.Dto;
using Intcomex.ProductsApi.Application.Services;
using Intcomex.ProductsApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

public class CategoryControllerTests
{
    private readonly Mock<CategoryService> _mockCategoryService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoryController _controller;
    private readonly new Mock<ILogger<CategoryController>> _mockLogger;

    public CategoryControllerTests()
    {
        _mockCategoryService = new Mock<CategoryService>(MockBehavior.Strict, null!);
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CategoryController>>();
        _controller = new CategoryController(_mockCategoryService.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ReturnsCreatedResult_WhenModelIsValid()
    {
        // Arrange
        var inputDto = new CategoryDto { CategoryName = "Test", Description = "Desc" };
        var createdDto = new CategoryDto { CategoryId = 1, CategoryName = "Test", Description = "Desc" };

        _mockCategoryService
            .Setup(s => s.CreateAsync(inputDto))
            .ReturnsAsync(createdDto);

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
        Assert.Equal(createdDto.CategoryId, ((CategoryDto)createdResult.Value).CategoryId);

        _mockCategoryService.Verify(s => s.CreateAsync(inputDto), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("CategoryName", "Required");

        // Act
        var result = await _controller.Create(new CategoryDto());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);

        _mockCategoryService.Verify(s => s.CreateAsync(It.IsAny<CategoryDto>()), Times.Never);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var categoryDto = new CategoryDto
        {
            CategoryId = categoryId,
            CategoryName = "Cat",
            Description = "Desc"
        };

        _mockCategoryService
            .Setup(s => s.GetByIdAsync(categoryId))
            .ReturnsAsync(categoryDto);

        // Act
        var result = await _controller.GetById(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategory = Assert.IsType<CategoryDto>(okResult.Value); // cambia aquí también
        Assert.Equal(categoryId, returnedCategory.CategoryId);

        _mockCategoryService.Verify(s => s.GetByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = 999;
        _mockCategoryService.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _controller.GetById(categoryId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        _mockCategoryService.Verify(s => s.GetByIdAsync(categoryId), Times.Once);
    }


    [Fact]
    public async Task Categorys_ReturnsOk_WithListOfCategoryDtos()
    {
        // Arrange
        var categories = new List<CategoryDto>
        {
            new CategoryDto { CategoryId = 1, CategoryName = "Cat1", Description = "Desc1" },
            new CategoryDto { CategoryId = 2, CategoryName = "Cat2", Description = "Desc2" }
        };

        _mockCategoryService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _controller.Categorys();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(okResult.Value);
        Assert.Collection(returnedCategories,
            item => Assert.Equal("Cat1", item.CategoryName),
            item => Assert.Equal("Cat2", item.CategoryName));

        _mockCategoryService.Verify(s => s.GetAllAsync(), Times.Once);
    }
    [Fact]
    public async Task Create_Returns500_WhenServiceThrowsException()
    {
        // Arrange
        var inputDto = new CategoryDto { CategoryName = "Cat", Description = "Desc" };
        _mockCategoryService
            .Setup(s => s.CreateAsync(inputDto))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _controller.Create(inputDto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Ha ocurrido un error al crear la categoría.", objectResult.Value);
    }
}

