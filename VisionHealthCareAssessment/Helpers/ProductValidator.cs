using FluentValidation;
using VisionHealthCareAssessment.Models;

namespace VisionHealthCareAssessment.Helpers
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductGroupId).NotEmpty();
        }
    }
}
