using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using BookShop.Data.Entities;
using BookShop.Services.Impl;
using Moq.EntityFrameworkCore;
using DbSetMockExtensions;

namespace BookShop.Services.Tests
{
    public class ProductServiceTests
    {
        //private readonly Mock<BookShopDbContext> _dbContextMock;
        //private readonly Mock<ILogger<ProductService>> _loggerMock;
        //private readonly Mock<IMapper> _mapperMock;
        //private readonly Mock<IConnectionMultiplexer> _connectionMultiplexerMock;
        //private readonly ProductService _productService;

        //public ProductServiceTests()
        //{
        //    _dbContextMock = new Mock<BookShopDbContext>();
        //    _loggerMock = new Mock<ILogger<ProductService>>();
        //    _mapperMock = new Mock<IMapper>();
        //    _connectionMultiplexerMock = new Mock<IConnectionMultiplexer>();

        //    _productService = new ProductService(
        //        _dbContextMock.Object,
        //        _loggerMock.Object,
        //        _mapperMock.Object,
        //        _connectionMultiplexerMock.Object);
        //}

        //[Fact]
        //public async Task RemoveAsync_CallsExecuteDeleteAsync()
        //{
        //    var productId = 1;
        //    var productEntities = new List<ProductEntity>
        //{
        //    new ProductEntity { Id = productId, Name = "Test Product" }
        //};
        //    SetMockDbContext(productEntities);

        //    await _productService.RemoveAsync(productId);

        //    _dbContextMock.Verify(db => db.Products.Remove(It.Is<ProductEntity>(p => p.Id == productId)), Times.Once);
        //    _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        //}

        //private void SetMockDbContext(IEnumerable<ProductEntity> entities)
        //{
        //    var dbSetMock = entities.CreateDbSetMock();
        //    _dbContextMock.Setup(x => x.Products).Returns(dbSetMock.Object);
        //}
    }
}
