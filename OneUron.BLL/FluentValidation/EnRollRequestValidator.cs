using FluentValidation;
using OneUron.BLL.DTOs.EnRollDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class EnRollRequestValidator : AbstractValidator<EnRollRequestDto>
    {
        public EnRollRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId không được để trống.");

            RuleFor(x => x.ResourceId)
                .NotEmpty()
                .WithMessage("ResourceId không được để trống.");

            RuleFor(x => x.EnrollDate)
                .NotEmpty()
                .WithMessage("EnrollDate không được để trống.");
                //.LessThanOrEqualTo(DateTime.UtcNow)
                //.WithMessage("EnrollDate không thể là ngày trong tương lai.");
        }
    }
}
