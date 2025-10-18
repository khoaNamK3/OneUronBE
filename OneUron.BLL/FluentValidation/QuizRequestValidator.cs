using FluentValidation;
using OneUron.BLL.DTOs.QuizDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class QuizRequestValidator : AbstractValidator<QuizRequestDto>
    {
        public QuizRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên bài quiz không được để trống.")
                .MaximumLength(255).WithMessage("Tên bài quiz không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả bài quiz không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.TotalQuestion)
                .GreaterThan(0).WithMessage("Số lượng câu hỏi phải lớn hơn 0.")
                .LessThanOrEqualTo(200).WithMessage("Số lượng câu hỏi không được vượt quá 200.");

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage("Thời gian làm bài không được để trống.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Loại quiz phải thuộc giá trị hợp lệ trong enum QuizType.");

            RuleFor(x => x.TotalPoints)
                .GreaterThan(0).WithMessage("Tổng điểm phải lớn hơn 0.")
                .LessThanOrEqualTo(1000).WithMessage("Tổng điểm không được vượt quá 1000.");

            RuleFor(x => x.PassScore)
                .GreaterThanOrEqualTo(0).WithMessage("Điểm đạt phải lớn hơn hoặc bằng 0.")
                .LessThanOrEqualTo(x => x.TotalPoints)
                .WithMessage("Điểm đạt không được vượt quá tổng điểm.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId là bắt buộc.");
        }
    }
}
