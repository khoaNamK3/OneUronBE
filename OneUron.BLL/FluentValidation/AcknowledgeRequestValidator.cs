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
           .NotEmpty().WithMessage("Ô này không được để trống.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Id của khóa học không hợp lệ.")
                .NotEqual(Guid.Empty).WithMessage("Id của khóa học không hợp lệ.");
        }
    }
}
