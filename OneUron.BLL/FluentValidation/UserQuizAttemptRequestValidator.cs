using FluentValidation;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class UserQuizAttemptRequestValidator : AbstractValidator<UserQuizAttemptRequestDto>
    {
        public UserQuizAttemptRequestValidator()
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("QuizId là bắt buộc.");

            //RuleFor(x => x.StartAt)
            //     .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.");
                 //.Must(x => x >= DateTime.UtcNow)
                 //.WithMessage("Thời gian bắt đầu không thể ở quá khứ.");


            RuleFor(x => x.FinishAt)
                .NotEmpty().WithMessage("Thời gian kết thúc không được để trống.")
                .GreaterThan(x => x.StartAt)
                .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");

         //   RuleFor(x => x.Point)
         //       .GreaterThanOrEqualTo(0).WithMessage("Điểm số không được âm.")
         //       .LessThanOrEqualTo(1000).WithMessage("Điểm số không được vượt quá 1000.");

         //   RuleFor(x => x.Accuracy)
         //.GreaterThanOrEqualTo(0).WithMessage("Độ chính xác (Accuracy) phải lớn hơn hoặc bằng 0.");
    
        }
    }
}
