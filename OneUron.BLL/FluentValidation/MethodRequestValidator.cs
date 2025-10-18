using FluentValidation;
using OneUron.BLL.DTOs.MethodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MethodRequestValidator : AbstractValidator<MethodRequestDto>
    {
        public MethodRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên phương pháp không được để trống.")
                .MaximumLength(255).WithMessage("Tên phương pháp không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("Độ khó phải thuộc giá trị hợp lệ trong enum MethodType.");

            RuleFor(x => x.TimeInfo)
                .NotEmpty().WithMessage("Thông tin thời gian không được để trống.")
                .MaximumLength(100).WithMessage("Thông tin thời gian không được vượt quá 100 ký tự.");
        }
    }
}
