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
              .NotEmpty().WithMessage("Title is required.")
              .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .LessThanOrEqualTo(x => x.EndDate.Date)
                .When(x => x.EndDate != default)
                .WithMessage("StartDate must be before or equal to EndDate.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThanOrEqualTo(x => x.StartDate.Date)
                .When(x => x.StartDate != default)
                .WithMessage("EndDate must be after or equal to StartDate.");

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
