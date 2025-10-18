using Net.payOS.Types;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IPaymentService
    {
        public Task<List<PaymentResponseDto>> GetAllPaymentAsync();

        public Task<PaymentResponseDto> GetPaymentbyIdAsync(Guid id);

        public  Task<List<PaymentResponseDto>> GetPaymentByUserIdAsync(Guid id);

        public  Task<PaymentResponseDto> CreateNewPayment(PaymentRequestDto payment);

        public  Task<PaymentResponseDto> UpdatePaymentByIdAsyn(Guid id, PaymentRequestDto request);

        public  Task<PaymentResponseDto> DeletePaymentByidAsync(Guid id);

        public PaymentResponseDto MapToDTO(Payment payment);

        public  Task<string> CreatePaymentLinkAsync(Guid memberShipPlanId, Guid userId);

        public  Task HandleWebhookAsync(WebhookType webhookData);



    }
}
