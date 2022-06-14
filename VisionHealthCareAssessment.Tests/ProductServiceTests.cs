using Moq;
using VisionHealthCareAssessment.DAL;
using VisionHealthCareAssessment.Exceptions;
using VisionHealthCareAssessment.Helpers;
using VisionHealthCareAssessment.Models;
using VisionHealthCareAssessment.Services;
using Xunit;

namespace VisionHealthCareAssessment.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_ValidProduct_RepositoryCalled()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();

            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                Name = "DummyProduct",
                Currency = "EUR",
                Price = 1,
                ProductGroupId = Guid.NewGuid()
            };

            productRepositoryMock.Setup(x => x.CreateProduct(product));

            IProductService productService = new ProductService(productRepositoryMock.Object, new ProductValidator());

            await productService.CreateOrUpdateProduct(product);

            productRepositoryMock.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_InvalidProduct_ThrowsException()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();

            Product product = new Product()
            {
                ProductId = Guid.NewGuid(),
                //Name = "DummyProduct",
                Currency = "EUR",
                Price = 1,
                ProductGroupId = Guid.NewGuid()
            };

            productRepositoryMock.Setup(x => x.CreateProduct(product));

            IProductService productService = new ProductService(productRepositoryMock.Object, new ProductValidator());

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => productService.CreateOrUpdateProduct(product));

            productRepositoryMock.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProduct_NonExistingProduct_ThrowsException()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();

            var nonExistingProductId = Guid.NewGuid();

            productRepositoryMock.Setup(x => x.GetProduct(nonExistingProductId)).ReturnsAsync((Product)null);

            IProductService productService = new ProductService(productRepositoryMock.Object, new ProductValidator());

            await Assert.ThrowsAsync<ProductNotFoundException>(() => productService.DeleteProduct(Guid.NewGuid()));

            productRepositoryMock.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Never);
        }
    }
}
