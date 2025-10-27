using FluentValidation;
using OneUron.BLL.DTOs.ScheduleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ScheduleSubjectRequestValidator : AbstractValidator<ScheduleSubjectRequestDto>
    {
        public ScheduleSubjectRequestValidator()
        {
            RuleFor(x => x.Title)
              .NotEmpty().WithMessage("Tiêu đề là bắt buộc.")
              .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 kí tự.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc.")
                .LessThanOrEqualTo(x => x.EndDate.Date)
                .When(x => x.EndDate != default)
                .WithMessage("Ngày bắt đầu phải bé hơn hoặc bằng ngày kết thúc.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Ngày kết thúc là bắt buộc.")
                .GreaterThanOrEqualTo(x => x.StartDate.Date)
                .When(x => x.StartDate != default)
                .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");

            //RuleFor(x => x.TotalTime)
            //    .NotEmpty().WithMessage("TotalTime is required.")
            //    .MaximumLength(50).WithMessage("TotalTime must not exceed 50 characters.");

            //RuleFor(x => x.AmountSubject)
            //    .GreaterThan(0).WithMessage("AmountSubject must be greater than 0.");

            //RuleFor(x => x.CreateAt)
            //    .NotEmpty().WithMessage("CreateAt is required.")
            //    .GreaterThanOrEqualTo(DateTime.UtcNow)
            //    .WithMessage("CreateAt must be the current time or in the future.");

            RuleForEach(x => x.subjectListRequest)
                .SetValidator(new SubjectListRequestValidator());
        }
    }
}
