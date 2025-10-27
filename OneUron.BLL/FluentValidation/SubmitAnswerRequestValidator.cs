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
                .WithMessage("Tên Quiz là bắt buộc .");

      
            RuleFor(x => x.AnswerList)
                .NotNull()
                .WithMessage("Danh sách câu trả lời không được để trống .")
                .Must(list => list.Any())
                .WithMessage("Danh sách câu trả lời ít nhất phải có 1 câu trả lời.");

          
            RuleForEach(x => x.AnswerList)
                .SetValidator(new ListAnswerRequestValidator());
        }
    }
}
