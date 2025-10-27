using FluentValidation;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class EvaluationQuestionRequestValidator : AbstractValidator<EvaluationQuestionRequestDto>
    {
        public EvaluationQuestionRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống.")
                .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("kiểu phải thuộc kiểu của câu hỏi hợp lệ.");

            RuleFor(x => x.EvaluationId)
                .NotEmpty().WithMessage("Đánh giá không được để trống ");
        }
    }
}
