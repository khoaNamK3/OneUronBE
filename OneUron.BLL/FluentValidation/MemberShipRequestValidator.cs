using FluentValidation;
using OneUron.BLL.DTOs.MemberShipDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MemberShipRequestValidator : AbstractValidator<MemberShipRequestDto>
    {
        public MemberShipRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");

        
            RuleFor(x => x.ExpiredDate)
                .NotEmpty().WithMessage("Ngày hết hạn không được để trống.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("Ngày hết hạn phải lớn hơn ngày bắt đầu.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Trạng thái hội viên không hợp lệ.");

     
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("người dùng không được để trống.");

         
            RuleFor(x => x.MemberShipPlanId)
                .NotEmpty().WithMessage("gói thành viên không được để trống.");
        }
    }
}
