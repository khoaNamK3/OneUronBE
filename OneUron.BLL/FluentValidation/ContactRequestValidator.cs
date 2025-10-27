using FluentValidation;
using OneUron.BLL.DTOs.ContactDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ContactRequestValidator : AbstractValidator<ContactRequestDto>
    {
        public ContactRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Phải đúng Email định dạng .");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^[0-9]{9,15}$").WithMessage("Số điện thoại phải bắt đầu từ 0 và có 9 đến 15 số");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Tin nhắn là bắt buộc .")
                .MaximumLength(500).WithMessage("Tin nhắn có tội đa là 500 kí tự");
        }
    }
}
