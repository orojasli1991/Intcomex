using FluentAssertions;
using Intcomex.ProductsApi.Application.Services;
using Intcomex.ProductsApi.Domain.Interfaces;
using Intcomex.ProductsApi.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Http;
using Intcomex.ProductsApi.Domain.Entities;
using Intcomex.ProductsApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Intcomex.ProductsApi.Tests.Application
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        public ProductServiceTests()
        {
            // Aquí deberías levantar la configuración real si hace falta
            var services = new ServiceCollection();

            // Configurar manualmente el repositorio real (opcional si usas DI)
            services.AddScoped<IProductRepository, ProductRepository>(); // Usa tu implementación real
            services.AddScoped<ProductService>(); // Servicio que contiene CreateAsync

            // Agrega cualquier configuración adicional necesaria
            services.AddHttpClient(); // Si tu servicio genera llamadas HTTP

            var provider = services.BuildServiceProvider();

            _productService = provider.GetRequiredService<ProductService>();
        }
    }

}




