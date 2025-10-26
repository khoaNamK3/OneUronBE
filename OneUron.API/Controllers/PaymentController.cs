
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<PaymentResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPayments()
        {
            var response = await _paymentService.GetAllPaymentAsync();

            return Ok(ApiResponse<List<PaymentResponseDto>>.SuccessResponse(response, "Get All Payment Successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetbyIdAsync(Guid id)
        {
            var response = await _paymentService.GetPaymentbyIdAsync(id);

            return Ok(ApiResponse<PaymentResponseDto>.SuccessResponse(response, "Get By Id Successfully"));
        }

        [HttpGet("get-all-by/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<List<PaymentResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentsByUserId(Guid userId)
        {
            var response = await _paymentService.GetPaymentByUserIdAsync(userId);

            return Ok(ApiResponse<List<PaymentResponseDto>>.SuccessResponse(response, "Get All Payment By User Sucessfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto request)
        {
            var response = await _paymentService.CreateNewPayment(request);

            return Ok(ApiResponse<PaymentResponseDto>.SuccessResponse(response, "Create new Payment Successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePaymentAsync(Guid id, [FromBody] PaymentRequestDto request)
        {
            var response = await _paymentService.UpdatePaymentByIdAsyn(id, request);

            return Ok(ApiResponse<PaymentResponseDto>.SuccessResponse(response, "Update Payment By Id Successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePaymentByIdAsync(Guid id)
        {
            var response = await _paymentService.DeletePaymentByidAsync(id);

            return Ok(ApiResponse<PaymentResponseDto>.SuccessResponse(response, "Delete Paymentt By Id Successfully"));
        }

        [HttpGet("create-payment-link-by/{memberShipPlanId}/{userId}")]
        public async Task<IActionResult> CreatePaymentLink(Guid memberShipPlanId, Guid userId)
        {
            var checkoutUrl = await _paymentService.CreatePaymentLinkAsync(memberShipPlanId, userId);

            return Ok(new
            {
                message = "Payment link created successfully.",
                checkoutUrl
            });
        }
        
        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] WebhookType webhookData)
        {
            await _paymentService.HandleWebhookAsync(webhookData);
            return Ok(new { message = "Webhook processed successfully." });
        }
    }
}
