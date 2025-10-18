using FluentValidation;
using Net.payOS;
using Net.payOS.Types;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MemberShipPlanRepo;
using OneUron.DAL.Repository.MemberShipRepo;
using OneUron.DAL.Repository.PaymentRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IValidator<PaymentRequestDto> _paymentValidator;
        private readonly IMemberShipRepository _memberShipRepository;
        private readonly PayOS _payOS;
        public PaymentService(
        IPaymentRepository paymentRepository,
        IMemberShipPlanRepository memberShipPlanRepository,
        IValidator<PaymentRequestDto> paymentValidator,
        IMemberShipRepository memberShipRepository,
        PayOS payOS)
        {
            _paymentRepository = paymentRepository;
            _memberShipPlanRepository = memberShipPlanRepository;
            _paymentValidator = paymentValidator;
            _memberShipRepository = memberShipRepository;
            _payOS = payOS;
        }

        public async Task<string> CreatePaymentLinkAsync(Guid memberShipPlanId, Guid userId)
        {
            if (memberShipPlanId == Guid.Empty || userId == Guid.Empty)
                throw new ApiException.BadRequestException("memberShipPlanId or userId is empty, please check again.");

            var plan = await _memberShipPlanRepository.GetByIdAsync(memberShipPlanId);
            if (plan == null)
                throw new ApiException.NotFoundException("Membership plan not found.");

            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)plan.Fee,
                description: $"Thanh toán: {plan.Name}",
                items: new List<ItemData> { new ItemData(plan.Name, 1, (int)plan.Fee) },
                cancelUrl: "https://yourfrontend.com/payment/cancel",
                returnUrl: "https://yourfrontend.com/payment/success"
            );

            var paymentLink = await _payOS.createPaymentLink(paymentData);

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                MemberShipPlanId = plan.Id,
                Amount = plan.Fee,
                Status = PaymentStatus.Pending,
                CreateAt = DateTime.UtcNow,
                OrderCode = orderCode
            };

            await _paymentRepository.AddAsync(payment);

            return paymentLink.checkoutUrl;
        }

        public async Task HandleWebhookAsync(WebhookType webhookData)
        {
            var verifiedData = _payOS.verifyPaymentWebhookData(webhookData);
            var orderCode = verifiedData.orderCode;

            if (orderCode == null)
                throw new ApiException.BadRequestException("Order code missing in webhook.");

            var payment = await _paymentRepository.GetByOrderCodeAsync(orderCode);
            if (payment == null)
                throw new ApiException.NotFoundException("Payment not found.");

            if (verifiedData.code == "00")
            {
                payment.Status = PaymentStatus.Paid;
                await _paymentRepository.UpdateAsync(payment);

                var plan = await _memberShipPlanRepository.GetByIdAsync(payment.MemberShipPlanId);
                if (plan == null)
                    throw new ApiException.NotFoundException("Membership plan not found.");

                var existingMemberShip = await _memberShipRepository.GetByUserIdAsync(payment.UserId);

                if (existingMemberShip != null)
                {
                    existingMemberShip.StartDate = DateTime.UtcNow;
                    existingMemberShip.ExpiredDate = DateTime.UtcNow.AddMonths(1);
                    existingMemberShip.Status = MemberShipStatus.Active;
                    existingMemberShip.MemberShipPlanId = plan.Id;

                    await _memberShipRepository.UpdateAsync(existingMemberShip);
                }
                else
                {
                    var newMemberShip = new MemberShip
                    {
                        Id = Guid.NewGuid(),
                        UserId = payment.UserId,
                        MemberShipPlanId = plan.Id,
                        StartDate = DateTime.UtcNow,
                        ExpiredDate = DateTime.UtcNow.AddMonths(1),
                        Status = MemberShipStatus.Active
                    };

                    await _memberShipRepository.AddAsync(newMemberShip);
                }
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                await _paymentRepository.UpdateAsync(payment);
            }
        }





        public async Task<List<PaymentResponseDto>> GetAllPaymentAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentAsync();

            if (!payments.Any())
                throw new ApiException.NotFoundException("No Payments Fond");

            var result = payments.Select(MapToDTO).ToList();

            return result;
        }

        public async Task<PaymentResponseDto> GetPaymentbyIdAsync(Guid id)
        {
            var existPayment = await _paymentRepository.GetByIdAsync(id);

            if (existPayment == null)
                throw new ApiException.NotFoundException("No Payments Found");

            var result = MapToDTO(existPayment);

            return result;
        }

        public async Task<List<PaymentResponseDto>> GetPaymentByUserIdAsync(Guid id)
        {
            var userPayment = await _paymentRepository.GetPaymentByUserIdAsync(id);

            if (userPayment == null)
                throw new ApiException.NotFoundException("User No Payments Found");

            var result = userPayment.Select(MapToDTO).ToList();
            return result;
        }

        public async Task<PaymentResponseDto> CreateNewPayment(PaymentRequestDto payment)
        {
            var validatorResult = await _paymentValidator.ValidateAsync(payment);

            if (!validatorResult.IsValid)
                throw new ApiException.ValidationException(validatorResult.Errors);

            var newPayment = MapToEntity(payment);

            await _paymentRepository.AddAsync(newPayment);

            var result = MapToDTO(newPayment);
            return result;
        }

        public async Task<PaymentResponseDto> UpdatePaymentByIdAsyn(Guid id, PaymentRequestDto request)
        {
            var existPayment = await _paymentRepository.GetByIdAsync(id);

            if (existPayment == null)
                throw new ApiException.NotFoundException("No Payment found");

            var validationResult = await _paymentValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existPayment.Amount = request.Amount;
            existPayment.CreateAt = request.CreateAt;
            existPayment.MemberShipPlanId = request.MemberShipPlanId;
            existPayment.Status = request.Status;
            existPayment.UserId = request.UserId;

            await _paymentRepository.UpdateAsync(existPayment);

            var result = MapToDTO(existPayment);
            return result;
        }

        public async Task<PaymentResponseDto> DeletePaymentByidAsync(Guid id)
        {
            var existPayment = await _paymentRepository.GetByIdAsync(id);

            if (existPayment == null)
                throw new ApiException.NotFoundException("No Payment found");

            var result = MapToDTO(existPayment);

            await _paymentRepository.DeleteAsync(existPayment);

            return result;
        }

        public Payment MapToEntity(PaymentRequestDto newPayment)
        {
            return new Payment
            {
                Amount = newPayment.Amount,
                CreateAt = newPayment.CreateAt,
                MemberShipPlanId = newPayment.MemberShipPlanId,
                Status = newPayment.Status,
                UserId = newPayment.UserId,
                OrderCode = newPayment.OrderCode
            };
        }

        public PaymentResponseDto MapToDTO(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                CreateAt = payment.CreateAt,
                MemberShipPlanId = payment.MemberShipPlanId,
                Status = payment.Status,
                UserId = payment.UserId,
                OrderCode = payment.OrderCode,
            };
        }
    }
}
