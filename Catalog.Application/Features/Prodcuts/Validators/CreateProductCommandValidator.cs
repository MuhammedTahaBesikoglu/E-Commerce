using Catalog.Application.Features.Prodcuts.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Prodcuts.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Ürün adı boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Ürün adı en fazla 200 karakter olabilir.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok adedi negatif olamaz.");

            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Geçerli bir kategori ID'si zorunludur.");
        }
    }
}
