using FluentValidation;
using OneUron.BLL.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequestDto>
    {

        public UpdatePaymentRequestValidator()
        {

            RuleFor(x => x.OrderCode)
                .NotEmpty().WithMessage("OrderCode is required.");


            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid payment status value.");
        }
    }
}
