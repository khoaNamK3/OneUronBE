using FluentValidation;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class MethodRuleConditionRequestValidator : AbstractValidator<MethodRuleConditionRequestDto>
    {
        public MethodRuleConditionRequestValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).WithMessage("Trọng số (Weight) phải lớn hơn hoặc bằng 0.")
                .LessThanOrEqualTo(1).WithMessage("Trọng số (Weight) không được vượt quá 1.");

            RuleFor(x => x.Effectiveness)
                .GreaterThanOrEqualTo(0).WithMessage("Hiệu quả (Effectiveness) phải lớn hơn hoặc bằng 0.")
                .LessThanOrEqualTo(1).WithMessage("Hiệu quả (Effectiveness) không được vượt quá 1.");

            //RuleFor(x => x.EvaluationId)
            //    .NotEmpty().WithMessage("Đánh giá là bắt buộc.");

            //RuleFor(x => x.EvaluationQuestionId)
            //    .NotEmpty().WithMessage("Câu hỏi đánh giá là bắt buộc.");

            RuleFor(x => x.ChoiceId)
                .NotEmpty().WithMessage("Không được để trống lựa chọn");
        }
    }
}
