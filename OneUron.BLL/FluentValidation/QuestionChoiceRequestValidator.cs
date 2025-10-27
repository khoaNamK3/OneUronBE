using FluentValidation;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class QuestionChoiceRequestValidator : AbstractValidator<QuestionChoiceRequestDto>
    {
        public QuestionChoiceRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nội dung lựa chọn (Name) không được để trống.")
                .MaximumLength(255).WithMessage("Nội dung lựa chọn không được vượt quá 255 ký tự.");

            RuleFor(x => x.IsCorrect)
                .NotNull().WithMessage("Trạng thái đúng/sai (IsCorrect) là bắt buộc.");

            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("Câu hỏi không được để trống");
        }
    }
}
