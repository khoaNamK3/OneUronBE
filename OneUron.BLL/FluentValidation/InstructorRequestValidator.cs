using FluentValidation;
using OneUron.BLL.DTOs.InstructorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class InstructorRequestValidator : AbstractValidator<InstructorRequestDto>
    {
        public InstructorRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên giảng viên không được để trống.")
                .MaximumLength(255).WithMessage("Tên giảng viên không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Experience)
                .NotEmpty().WithMessage("Kinh nghiệm không được để trống.")
                .MaximumLength(500).WithMessage("Kinh nghiệm không được vượt quá 500 ký tự.");

            RuleFor(x => x.Contact)
                .NotEmpty().WithMessage("Thông tin liên hệ không được để trống.")
                .Matches(@"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$|^(\+?\d{7,15})$")
                .WithMessage("Liên hệ phải là email hoặc số điện thoại hợp lệ.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("CourseId là bắt buộc.");
        }
    }
}
