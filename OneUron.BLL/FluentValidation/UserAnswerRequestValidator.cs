using FluentValidation;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class UserAnswerRequestValidator : AbstractValidator<UserAnswerRequestDto>
    {
        public UserAnswerRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId là bắt buộc.");

            RuleFor(x => x.ChoiceId)
                .NotEmpty().WithMessage("ChoiceId là bắt buộc.");

            RuleFor(x => x.EvaluationQuestionId)
                .NotEmpty().WithMessage("EvaluationQuestionId là bắt buộc.");
        }
    }
}
