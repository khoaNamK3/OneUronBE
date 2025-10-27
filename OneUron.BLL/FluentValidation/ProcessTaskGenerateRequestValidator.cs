using FluentValidation;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ProcessTaskGenerateRequestValidator : AbstractValidator<ProcessTaskGenerateRequest>
    {
        public ProcessTaskGenerateRequestValidator()
        {
            RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Tiêu đề  không được để trống.")
          .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Mô tả  không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Giá trị phải lớn hơn 0.");

            RuleFor(x => x.StartTime)
                  .NotEmpty().WithMessage("Thời gian bắt đầu  không được để trống.");
        }
    }
}
