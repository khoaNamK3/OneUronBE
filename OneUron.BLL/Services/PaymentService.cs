using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Net.payOS;
using Net.payOS.Types;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
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
        private readonly IValidator<UpdatePaymentRequestDto> _updatePaymentValidator;
        private readonly PayOS _payOS;
        public PaymentService(
        IPaymentRepository paymentRepository,
        IMemberShipPlanRepository memberShipPlanRepository,
        IValidator<PaymentRequestDto> paymentValidator,
        IMemberShipRepository memberShipRepository,
        IValidator<UpdatePaymentRequestDto> updatePaymentValidator,
        PayOS payOS)
        {
            _paymentRepository = paymentRepository;
            _memberShipPlanRepository = memberShipPlanRepository;
            _paymentValidator = paymentValidator;
            _memberShipRepository = memberShipRepository;
            _updatePaymentValidator = updatePaymentValidator;
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
                cancelUrl: "https://studypath.vercel.app/payment/status?Success=cancel",
                returnUrl: "https://studypath.vercel.app/payment/status"
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


        public async Task<PaymentResponseDto> UpdatePaymentStatusAsync(UpdatePaymentRequestDto requestDto)
        {
            if (requestDto == null)
                throw new ApiException.BadRequestException("Thông tin cập nhật hóa đơn không được để trống");

            var validationResult =await _updatePaymentValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var existPayment = await _paymentRepository.GetByOrderCodeAsync(requestDto.OrderCode);

            if (existPayment == null)
                throw new ApiException.NotFoundException("Không tìm thấy Payment ");

            existPayment.Status = requestDto.Status;
         
            await _paymentRepository.UpdateAsync(existPayment);

            var result = MapToDTO(existPayment);

            // create memberShip 
            if(result.Status == PaymentStatus.Paid)
            {
                var existPlan = await _memberShipPlanRepository.GetByIdAsync(result.MemberShipPlanId);
              
                if (existPlan == null)
                    throw new ApiException.NotFoundException($"Không tìm thấy gói hội viên với ID: {result.MemberShipPlanId}");
                
                DateTime expiredDate;

                if (existPlan.memberShipPlanType == MemberShipPlanType.MONTH)
                {
                    expiredDate = DateTime.UtcNow.AddMonths(1);
                }
                else if (existPlan.memberShipPlanType == MemberShipPlanType.YEAR)
                {
                    expiredDate = DateTime.UtcNow.AddYears(1);
                }
                else
                {
                    throw new ApiException.BadRequestException("Loại gói hội viên không hợp lệ.");
                }

                var newMemberShip = new MemberShipRequestDto
                {
                        UserId = result.UserId,
                        MemberShipPlanId = result.MemberShipPlanId,
                        Status = MemberShipStatus.Active,
                        StartDate = DateTime.UtcNow,
                        ExpiredDate = expiredDate,
                };
            }

            return result;
        }

        public async Task<PaymentChartResponse> CalculateTotalPaymentEachMonthOfYearAsync(int year)
        {
            var monthResponse = await _paymentRepository.CalculateTotalPaymentEachMonthOfYearAsync(year);

            if (monthResponse == null || !monthResponse.Any())
                throw new ApiException.NotFoundException("Không tìm thấy hóa đơn vào khoảng thời gian này");


            var result = monthResponse.Select(MapperMonthPaymentSummary).ToList();
            var allMonths = Enumerable.Range(1, 12)
            .Select(m => new MonthlyPaymentSummary
            {
                Month = m,
                TotalAmount = result.FirstOrDefault(r => r.Month == m)?.TotalAmount ?? 0
            }).ToList();

            var allYears = await _paymentRepository.GetAllYearAsync();

            return new PaymentChartResponse
            {
                ChartData = allMonths,
                Year = allYears
            };
        }

        public async Task<List<PaymentResponseDto>> RecentPaymentAsync()
        {
            var recentPayments = await _paymentRepository.RecentPaymentAsync();

            if (!recentPayments.Any())
                throw new ApiException.NotFoundException("No Recent Bill Found");

            var result = recentPayments.Select(MapToDTO).ToList();

            return result;
        }

        public MonthlyPaymentSummary MapperMonthPaymentSummary(Payment payment)
        {
            return new MonthlyPaymentSummary
            {
                Month = payment.CreateAt.Month,
                TotalAmount = payment.Amount,
            };
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
