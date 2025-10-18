using FluentValidation;
using OneUron.BLL.DTOs.MethodConDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MethodConRequestValidator : AbstractValidator<MethodConRequestDto>
    {
        public MethodConRequestValidator()
        {
            RuleFor(x => x.Con)
                .NotEmpty().WithMessage("Nội dung điều kiện (Con) không được để trống.")
                .MaximumLength(500).WithMessage("Nội dung điều kiện không được vượt quá 500 ký tự.");

            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("MethodId là bắt buộc.");
        }
    }
}
