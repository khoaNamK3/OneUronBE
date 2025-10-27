using FluentValidation;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class StudyMethodRequestValidator : AbstractValidator<StudyMethodRequestDto>
    {
        public StudyMethodRequestValidator()
        {
            //RuleFor(x => x.IsDeleted)
            //    .NotNull().WithMessage("Trạng thái xóa (IsDeleted) là bắt buộc.");

            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("Phương pháp học là bắt buộc.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Người dùng là bắt buộc.");
        }
    }
}
