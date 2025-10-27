using FluentValidation;
using OneUron.BLL.DTOs.ChoiceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ChoiceRequestValidator : AbstractValidator<ChoiceRequestDto>
    {
        public ChoiceRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Tựa đề không được để trống.")
                .MaximumLength(200)
                .WithMessage("Tựa đề không được vượt quá 200 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Mô tả không được vượt quá 500 ký tự.");

            RuleFor(x => x.EvaluationQuestionId)
                .NotEmpty()
                .WithMessage("Câu hỏi đánh giá không được để trống.");
        }
    }
}
