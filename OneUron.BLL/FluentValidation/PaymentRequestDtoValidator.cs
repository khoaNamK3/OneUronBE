using FluentValidation;
using OneUron.BLL.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class PaymentRequestDtoValidator : AbstractValidator<PaymentRequestDto>
    {
        public PaymentRequestDtoValidator()
        {
          
            RuleFor(x => x.CreateAt)
                .NotEmpty()
                .WithMessage("Thời gian tạo thanh toán không được để trống.");

      
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Số tiền thanh toán phải lớn hơn 0.");

          
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Trạng thái thanh toán không hợp lệ.");

          
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Người dùng không được để trống.");

        
            RuleFor(x => x.MemberShipPlanId)
                .NotEmpty()
                .WithMessage("Gói hội viên không được để trống.");
        }
    }
}
