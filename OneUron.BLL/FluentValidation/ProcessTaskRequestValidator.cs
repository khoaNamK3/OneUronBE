using FluentValidation;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ProcessTaskRequestValidator : AbstractValidator<ProcessTaskRequestDto>
    {
        public ProcessTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề công việc không được để trống.")
                .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage("Ghi chú không được vượt quá 500 ký tự.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.")
                .Must(start => start >= DateTime.UtcNow)
                .WithMessage("Thời gian bắt đầu phải lớn hơn hoặc bằng thời điểm hiện tại.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("Thời gian kết thúc không được để trống.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");

            RuleFor(x => x.ProcessId)
                .NotEmpty().WithMessage("ProcessId là bắt buộc.");
        }
    }
}
