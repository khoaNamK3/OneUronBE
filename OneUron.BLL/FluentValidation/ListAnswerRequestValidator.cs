using FluentValidation;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ListAnswerRequestValidator : AbstractValidator<ListAnswerRequest>
    {
        public ListAnswerRequestValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty()
                .WithMessage("QuestionId is required.");

            RuleFor(x => x.QuestionChoiceId)
                .NotEmpty()
                .WithMessage("QuestionChoiceId is required.");
        }
    }
}