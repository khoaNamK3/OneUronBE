
using FluentValidation;
using OneUron.BLL.DTOs.SubjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class SubjectRequestValidator : AbstractValidator<SubjectRequestDto>
    {
        public SubjectRequestValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Tên môn học không được để trống.")
           .MaximumLength(255).WithMessage("Tên môn học không được vượt quá 255 ký tự.");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Độ ưu tiên (Priority) phải thuộc enum SubjectType hợp lệ.");

            RuleFor(x => x.ScheduleId)
                .NotEmpty().WithMessage("ScheduleId là bắt buộc.");

     
            //When(x => x.ProcessId.HasValue, () =>
            //{
            //    RuleFor(x => x.ProcessId)
            //        .Must(id => id != Guid.Empty)
            //        .WithMessage("ProcessId không được để trống khi gán Subject vào Process.");
            //});
        }
    }
}
