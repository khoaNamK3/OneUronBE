using FluentValidation;
using OneUron.BLL.DTOs.ResourceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.FluentValidation
{
    public class ResourceRequestValidator : AbstractValidator<ResourceRequestDto>
    {
        public ResourceRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề tài nguyên không được để trống.")
                .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự.");

            RuleFor(x => x.Organization)
                .NotEmpty().WithMessage("Tên tổ chức không được để trống.")
                .MaximumLength(255).WithMessage("Tên tổ chức không được vượt quá 255 ký tự.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Mô tả tài nguyên không được để trống.")
                .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Ảnh minh họa không được để trống.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Đường dẫn ảnh không hợp lệ.");

            RuleFor(x => x.Star)
                .InclusiveBetween(0, 5).WithMessage("Điểm đánh giá (Star) phải nằm trong khoảng từ 0 đến 5.");

            RuleFor(x => x.Reviews)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng đánh giá (Reviews) không được âm.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá (Price) không được âm.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Loại tài nguyên (Type) phải thuộc enum ResourceType hợp lệ.");
        }
    }
}
