using FluentValidation;
using OneUron.BLL.DTOs.MethodProDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MethodProRequestValidator : AbstractValidator<MethodProRequestDto>
    {
        public MethodProRequestValidator()
        {
            RuleFor(x => x.Pro)
                .NotEmpty().WithMessage("Ưu điểm (Pro) không được để trống.")
                .MaximumLength(500).WithMessage("Ưu điểm không được vượt quá 500 ký tự.");

            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("phương pháp học là bắt buộc.");
        }
    }
}
