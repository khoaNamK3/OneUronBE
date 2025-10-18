using FluentValidation;
using OneUron.BLL.DTOs.ProcessDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ProcessRequestValidator : AbstractValidator<ProcessRequestDto>
    {
        public ProcessRequestValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Ngày thực hiện không được để trống.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .WithMessage("Ngày thực hiện không thể nhỏ hơn ngày hiện tại.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            //RuleFor(x => x.ScheduleId)
            //    .NotEmpty().WithMessage("ScheduleId là bắt buộc.");
        }
    }
}
