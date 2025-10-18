using FluentValidation;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class EvaluationQuestionRequestValidator : AbstractValidator<EvaluationQuestionRequestDto>
    {
        public EvaluationQuestionRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title không được để trống.")
                .MaximumLength(255).WithMessage("Title không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description không được để trống.")
                .MaximumLength(1000).WithMessage("Description không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Type phải thuộc enum EvaluationQuestionType hợp lệ.");

            RuleFor(x => x.EvaluationId)
                .NotEmpty().WithMessage("EvaluationId là bắt buộc.");
        }
    }
}
