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
                .WithMessage("Title không được để trống.")
                .MaximumLength(200)
                .WithMessage("Title không được vượt quá 200 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description không được vượt quá 500 ký tự.");

            RuleFor(x => x.EvaluationQuestionId)
                .NotEmpty()
                .WithMessage("EvaluationQuestionId không được để trống.");
        }
    }
}
