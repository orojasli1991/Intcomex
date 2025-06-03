using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Infrastructure.Persistence;
using Intcomex.ProductsApi.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Linq;

public class CategoryRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly CategoryRepository _repository;
    private readonly Mock<ILogger<CategoryRepository>> _mockLogger;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<ILogger<CategoryRepository>>();
        _repository = new CategoryRepository(_context, _mockLogger.Object);

        // Seed de datos opcional
        _context.Categories.AddRange(
            new Category { CategoryId = 0, CategoryName = "SERVIDORES" },
            new Category { CategoryId = 0, CategoryName = "CLOUD" }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories()
    {
        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(0, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCategory()
    {
        var result = await _repository.GetByIdAsync(2);

        Assert.NotNull(result);
        Assert.Equal("CLOUD", result.CategoryName);
    }

    [Fact]
    public async Task CreateAsync_AddsCategorySuccessfully()
    {
        var newCategory = new Category { CategoryName = "SERVIDORES" };

        var result = await _repository.CreateAsync(newCategory);

        Assert.NotNull(result);
        Assert.True(result.CategoryId > 0);

        var fromDb = await _context.Categories.FindAsync(result.CategoryId);
        Assert.Equal("SERVIDORES", fromDb.CategoryName);
    }
}

