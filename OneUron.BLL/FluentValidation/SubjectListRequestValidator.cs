using FluentValidation;
using OneUron.BLL.DTOs.ScheduleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class SubjectListRequestValidator : AbstractValidator<SubjectListRequest>
    {
        public SubjectListRequestValidator()
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Subject name is required.")
                .MaximumLength(200).WithMessage("Subject name must not exceed 200 characters.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid subject priority.");
        }
    }
}
