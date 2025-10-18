using FluentValidation;
using OneUron.BLL.DTOs.AnswerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class AnswerRequestValidator : AbstractValidator<AnswerRequestDto>
    {
        public AnswerRequestValidator()
        {
            RuleFor(x => x.QuestionId)
           .NotEmpty()
           .WithMessage("QuestionId not null.");

            RuleFor(x => x.QuestionChoiceId)
                .NotEmpty()
                .WithMessage("QuestionChoiceId not null.");

            RuleFor(x => x.UserQuizAttemptId)
                .NotEmpty()
                .WithMessage("UserQuizAttemptId not null.");
        }
    }
}
