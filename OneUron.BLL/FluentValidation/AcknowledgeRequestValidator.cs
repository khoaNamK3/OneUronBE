using FluentValidation;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class AcknowledgeRequestValidator : AbstractValidator<AcknowledgeRequestDto>
    {
        public AcknowledgeRequestValidator()
        {
            RuleFor(x => x.Text)
           .NotEmpty().WithMessage("Text must not be empty.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId is required.")
                .NotEqual(Guid.Empty).WithMessage("CourseId is invalid.");
        }
    }
}
