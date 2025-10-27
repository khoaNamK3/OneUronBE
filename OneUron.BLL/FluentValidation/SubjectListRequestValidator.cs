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
                .NotEmpty().WithMessage("Tên Môn học là bắt buộc.")
                .MaximumLength(200).WithMessage("Tên môn học không quá 200 kí tự");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("mức độ ưu tiên của của subject phải đúng kiểu dữ liệu.");
        }
    }
}
