using Microsoft.EntityFrameworkCore;
using VisionHealthCareAssessment.Models;

namespace VisionHealthCareAssessment.DAL
{
    public interface IProductRepository
    {
        Task CreateProduct(Product product);
        Task DeleteProduct(Guid productId);
        Task<Product> GetProduct(Guid productId);
        Task UpdateProduct(Product product);
        List<Product> GetProducts(string nameFilter);
        Task CreateOrUpdateProducts(List<Product> products);
    }

    public class ProductRepository : IProductRepository
    {
        private ProductContext _productContext;

        public ProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public List<Product> GetProducts(string nameFilter)
        {
            IEnumerable<Product> products = _productContext.Products;
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                products = products.Where(x => x.Name.Contains(nameFilter));
            }
            return products.ToList();
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await _productContext.Products.SingleOrDefaultAsync(x => x.ProductId.Equals(productId));
        }

        public async Task CreateProduct(Product product)
        {
            await _productContext.Products.AddAsync(product);
           
            await _productContext.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _productContext.Products.Update(product);

            await _productContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid productId)
        {
            var product = await GetProduct(productId);

            _productContext.Products.Remove(product);

            await _productContext.SaveChangesAsync();
        }

        public async Task CreateOrUpdateProducts(List<Product> products)
        {
            //first, get productIds of the products that already exist in the database.
            var existingProducts = _productContext.Products.Where(x => products.Select(p => p.ProductId).Contains(x.ProductId)).Select(x => x.ProductId).ToList();

            //get list of new products, batch insert those.
            var newProducts = products.Where(x => !existingProducts.Contains(x.ProductId));
            await _productContext.Products.AddRangeAsync(newProducts);

            //get all products that are not new, batch update those.
            var updateProducts = products.Except(newProducts);
            _productContext.Products.UpdateRange(updateProducts);

            await _productContext.SaveChangesAsync();
        }
    }
}
