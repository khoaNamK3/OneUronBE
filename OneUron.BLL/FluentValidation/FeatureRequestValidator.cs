using FluentValidation;
using OneUron.BLL.DTOs.FeatureDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class FeatureRequestValidator : AbstractValidator<FeatureRequestDto>
    {
        public FeatureRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên tính năng không được để trống.")
                .MinimumLength(3).WithMessage("Tên tính năng phải có ít nhất 3 ký tự.")
                .MaximumLength(100).WithMessage("Tên tính năng không được vượt quá 100 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MinimumLength(10).WithMessage("Mô tả phải có ít nhất 10 ký tự.")
                .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
        }
    }
}
