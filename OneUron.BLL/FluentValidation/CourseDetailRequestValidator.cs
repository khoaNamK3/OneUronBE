using FluentValidation;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class CourseDetailRequestValidator : AbstractValidator<CourseDetailRequestDto>
    {
        public CourseDetailRequestValidator()
        {
            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Duration không được để trống.");

            RuleFor(x => x.Level)
                .NotEmpty()
                .WithMessage("Level không được để trống.");

            RuleFor(x => x.Students)
                .NotEmpty()
                .WithMessage("Students không được để trống.");

            RuleFor(x => x.Reviews)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reviews phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price phải lớn hơn 0.");

            RuleFor(x => x.ResourceId)
                .NotEmpty()
                .WithMessage("ResourceId không được để trống.");
        }
    }
}
