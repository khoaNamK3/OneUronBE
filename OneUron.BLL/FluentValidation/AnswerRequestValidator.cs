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
           .WithMessage("Câu hỏi không được để trống.");

            RuleFor(x => x.QuestionChoiceId)
                .NotEmpty()
                .WithMessage("Lựa chọn của câu hỏi không được để trống.");

            RuleFor(x => x.UserQuizAttemptId)
                .NotEmpty()
                .WithMessage("Câu trả lời quiz không được để trống.");
        }
    }
}
