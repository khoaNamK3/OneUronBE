using FluentValidation;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MethodRuleRequestValidator : AbstractValidator<MethodRuleRequestDto>
    {
        public MethodRuleRequestValidator()
        {
            RuleFor(x => x.MethodId)
                .NotEmpty().WithMessage("MethodId là bắt buộc.");

            RuleFor(x => x.MethodRuleConditionId)
                .NotEmpty().WithMessage("MethodRuleConditionId là bắt buộc.");
        }
    }
}
