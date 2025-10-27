using FluentValidation;
using OneUron.BLL.DTOs.ScheduleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ScheduleRequestValidator : AbstractValidator<ScheduleRequestDto>
    {
        public ScheduleRequestValidator()
        {
            RuleFor(x => x.Title)
             .NotEmpty().WithMessage("Tiêu đề lịch học không được để trống.")
             .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.")
                .Must(start => start >= DateTime.UtcNow.Date)
                .WithMessage("Ngày bắt đầu phải từ hôm nay trở đi.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Ngày kết thúc không được để trống.")
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate != default)
                .WithMessage("Ngày kết thúc phải sau ngày bắt đầu.");

            RuleFor(x => x.TotalTime)
                .NotEmpty().WithMessage("Tổng thời gian không được để trống.")
                .MaximumLength(100).WithMessage("Tổng thời gian không được vượt quá 100 ký tự.");

            RuleFor(x => x.AmountSubject)
                .GreaterThan(0).WithMessage("Số lượng môn học phải lớn hơn 0.")
                .LessThanOrEqualTo(100).WithMessage("Số lượng môn học không được vượt quá 100.");

            RuleFor(x => x.CreateAt)
                .NotEmpty().WithMessage("Ngày tạo là bắt buộc.")
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Ngày tạo phải là hiện tại hoặc trong tương lai (không sớm hơn 10 phút so với hiện tại).");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Người dùng là bắt buộc.");
        }
    }
}
