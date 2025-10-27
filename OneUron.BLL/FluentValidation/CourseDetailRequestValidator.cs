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
                .WithMessage("Thời gian kéo dài  không được để trống.");

            RuleFor(x => x.Level)
                .NotEmpty()
                .WithMessage("Mức độ  không được để trống.");

            RuleFor(x => x.Students)
                .NotEmpty()
                .WithMessage("Số lượng học sinh không được để trống.");

            RuleFor(x => x.Reviews)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Số lượng Reviews phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Giá tiền phải lớn hơn 0.");

            RuleFor(x => x.ResourceId)
                .NotEmpty()
                .WithMessage("Khóa học không được để trống.");
        }
    }
}
