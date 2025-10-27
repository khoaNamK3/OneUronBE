using FluentValidation;
using OneUron.BLL.DTOs.EvaluationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class EvaluationRequestValidator : AbstractValidator<EvaluationRequestDto>
    {
        public EvaluationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên đánh giá  không được để trống.")
                .MaximumLength(200)
                .WithMessage("Tên đánh giá  không được vượt quá 200 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Mô tả không được để trống.")
                .MaximumLength(500)
                .WithMessage("Mô tả không được vượt quá 500 ký tự.");

            RuleFor(x => x.IsDeleted)
                .NotNull()
                .WithMessage("Giá trị đã xóa không được null.");
        }
    }
}
