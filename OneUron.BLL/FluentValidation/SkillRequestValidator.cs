using FluentValidation;
using OneUron.BLL.DTOs.SkillDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class SkillRequestValidator : AbstractValidator<SkillRequestDto>
    {
        public SkillRequestValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Nội dung kỹ năng (Text) không được để trống.")
                .MaximumLength(255).WithMessage("Nội dung kỹ năng không được vượt quá 255 ký tự.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId là bắt buộc.");
        }
    }
}
