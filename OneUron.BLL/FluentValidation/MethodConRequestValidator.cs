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
                .NotEmpty().WithMessage("Nội dung tác hại  (Con) không được để trống.")
                .MaximumLength(500).WithMessage("Nội dung tác hại không được vượt quá 500 ký tự.");

            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("phương pháp học là bắt buộc.");
        }
    }
}
