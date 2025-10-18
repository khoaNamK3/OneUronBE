using FluentValidation;
using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MemberShipPlanRequestValidator : AbstractValidator<MemberShipPlanRequestDto>
    {
        public MemberShipPlanRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên gói hội viên không được để trống.")
                .MaximumLength(100).WithMessage("Tên gói hội viên không được vượt quá 100 ký tự.");

            RuleFor(x => x.Fee)
                .GreaterThan(0).WithMessage("Phí gói hội viên phải lớn hơn 0.");

            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Thời hạn gói hội viên không được để trống.")
                .MaximumLength(50).WithMessage("Thời hạn gói hội viên không được vượt quá 50 ký tự.");
        }
    }
}
