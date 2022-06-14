using FluentValidation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VisionHealthCareAssessment.DAL;
using VisionHealthCareAssessment.Models;

namespace VisionHealthCareAssessment.Services
{
    public interface IImportService
    {
        Task<ImportReport> Import();
    }

    public class ImportService : IImportService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<Product> _validator;
        private readonly ImportSettings _importSettings;

        public ImportService(IProductRepository productRepository, IValidator<Product> validator, IOptions<ImportSettings> importSettings)
        {
            _productRepository = productRepository;
            _validator = validator;
            _importSettings = importSettings.Value;
        }

        public async Task<ImportReport> Import()
        {
            var json = await File.ReadAllTextAsync($"{Directory.GetCurrentDirectory()}/{_importSettings.Path}");
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);

            List<Product> validProducts = new List<Product>();
            ImportReport report = new ImportReport();

            foreach (var product in products)
            {
                var validationResult = _validator.Validate(product);
                if (validationResult.IsValid)
                {
                    validProducts.Add(product);
                    continue;
                }

                report.ValidationReport.Add($"{product.ProductId}, {string.Join(',', validationResult.Errors.Select(x => x.ErrorMessage))}");
            }

            await _productRepository.CreateOrUpdateProducts(validProducts);

            report.ProductsReadCount = products.Count;
            report.ProductsInsertedCount = validProducts.Count;
            return report;
        }
    }
}
