using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using OneUron.BLL.Until;
using OneUron.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class PayOsService
    {
        private readonly PayOS _payOS;

        public PayOsService(IOptions<PayOsSettings> options)
        {
            var settings = options.Value;
            _payOS = new PayOS(settings.ClientId, settings.ApiKey, settings.CheckSum);
        }

        public async Task<CreatePaymentResult> CreatePaymentLinkAsync(PaymentData data)
        {
            return await _payOS.createPaymentLink(data);
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInfoAsync(int id)
        {
            return await _payOS.getPaymentLinkInformation(id);
        }

        public async Task<PaymentLinkInformation> CancelPaymentLinkAsync(int id, string reason)
        {
            return await _payOS.cancelPaymentLink(id, reason);
        }

        public async Task<string> ConfirmWebhookAsync(string url)
        {
            return await _payOS.confirmWebhook(url);
        }

        public WebhookData VerifyPaymentWebhook(WebhookType type)
        {
            return _payOS.verifyPaymentWebhookData(type);
        }
    }

}
