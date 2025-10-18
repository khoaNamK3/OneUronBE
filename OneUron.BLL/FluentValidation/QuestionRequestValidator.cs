using FluentValidation;
using OneUron.BLL.DTOs.QuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class QuestionRequestValidator : AbstractValidator<QuestionRequestDto>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên câu hỏi không được để trống.")
                .MaximumLength(255).WithMessage("Tên câu hỏi không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả câu hỏi không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Point)
                .GreaterThan(0).WithMessage("Điểm số phải lớn hơn 0.")
                .LessThanOrEqualTo(100).WithMessage("Điểm số không được vượt quá 100.");

            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("QuizId là bắt buộc.");
        }
    }
}
