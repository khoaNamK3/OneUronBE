using FluentValidation;
using OneUron.BLL.DTOs.TechniqueDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class TechniqueRequestValidator : AbstractValidator<TechniqueRequestDto>
    {
        public TechniqueRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên kỹ thuật (Name) không được để trống.")
                .MaximumLength(255).WithMessage("Tên kỹ thuật không được vượt quá 255 ký tự.");

            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("MethodId là bắt buộc.");
        }
    }

}
