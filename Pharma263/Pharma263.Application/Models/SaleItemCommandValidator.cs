using FluentValidation;
using Pharma263.Domain.Models.Dtos;

namespace Pharma263.Application.Models
{
    public class SaleItemCommandValidator : AbstractValidator<SalesItemDto>
    {
        public SaleItemCommandValidator()
        {
            RuleFor(p => p.Price)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");

            RuleFor(p => p.Quantity)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");

            RuleFor(p => p.Amount)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than zero");

            RuleFor(p => p.StockId)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .GreaterThan(0);
        }
    }
}
