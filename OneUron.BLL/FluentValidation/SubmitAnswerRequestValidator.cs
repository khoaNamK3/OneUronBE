using FluentValidation;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class SubmitAnswerRequestValidator : AbstractValidator<SubmitAnswerRequest>
    {
        public SubmitAnswerRequestValidator()
        {
          
            RuleFor(x => x.QuizId)
                .NotEmpty()
                .WithMessage("QuizId is required.");

      
            RuleFor(x => x.AnswerList)
                .NotNull()
                .WithMessage("AnswerList cannot be null.")
                .Must(list => list.Any())
                .WithMessage("AnswerList must contain at least one answer.");

          
            RuleForEach(x => x.AnswerList)
                .SetValidator(new ListAnswerRequestValidator());
        }
    }
}
