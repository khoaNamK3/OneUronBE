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
                .WithMessage("Câu Hỏi không được để trống.");

            RuleFor(x => x.QuestionChoiceId)
                .NotEmpty()
                .WithMessage("Lựa chọn câu hỏi là bắt buộc.");
        }
    }
}