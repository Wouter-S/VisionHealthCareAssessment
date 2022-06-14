using FluentValidation;
using VisionHealthCareAssessment.DAL;
using VisionHealthCareAssessment.Exceptions;
using VisionHealthCareAssessment.Models;

namespace VisionHealthCareAssessment.Services
{
    public interface IProductService
    {
        Task CreateOrUpdateProduct(Product product);
        Task DeleteProduct(Guid productId);
        Task<Product> GetProduct(Guid productId);
        List<Product> GetProducts(string nameFilter);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<Product> _validator;

        public ProductService(IProductRepository productRepository, IValidator<Product> validator)
        {
            _productRepository = productRepository;
            _validator = validator;
        }

        public List<Product> GetProducts(string nameFilter) => _productRepository.GetProducts(nameFilter);

        public async Task<Product> GetProduct(Guid productId) =>
            await _productRepository.GetProduct(productId) ?? throw new ProductNotFoundException();

        public async Task CreateOrUpdateProduct(Product product)
        {
            var validationResult = _validator.Validate(product);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingProduct = await _productRepository.GetProduct(product.ProductId.Value);
            if (existingProduct != null)
            {
                await _productRepository.UpdateProduct(product);
            }
            else
            {
                await _productRepository.CreateProduct(product);
            }
        }

        public async Task DeleteProduct(Guid productId)
        {
            var existingProduct = await _productRepository.GetProduct(productId);
            if (existingProduct == null)
            {
                throw new ProductNotFoundException();
            }

            await _productRepository.DeleteProduct(productId);
        }
    }
}
