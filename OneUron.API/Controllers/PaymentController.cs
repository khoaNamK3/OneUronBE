
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/payment")]
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

        [HttpPut("update-status")]
        [ProducesResponseType(typeof(ApiResponse<PaymentResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePaymentStatusAsync([FromBody] UpdatePaymentRequestDto requestDto)
        {
            var response = await _paymentService.UpdatePaymentStatusAsync(requestDto);

            return Ok(ApiResponse<PaymentResponseDto>.SuccessResponse(response, "update Payment status Successfully"));
        }

        [HttpGet("get-payment-of-year")]
        [ProducesResponseType(typeof(ApiResponse<PaymentChartResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CalculatePaymentEachMonthOfYearAsync([FromQuery] int year)
        {
            var response = await _paymentService.CalculateTotalPaymentEachMonthOfYearAsync(year);
            return Ok(ApiResponse<PaymentChartResponse>.SuccessResponse(response, "Calculate Payment of Year Successfully"));
        }

        [HttpGet("get-recent-payment")]
        [ProducesResponseType(typeof(ApiResponse<List<PaymentResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentPaymentAsync()
        {
            var response = await _paymentService.RecentPaymentAsync();
            return Ok(ApiResponse<List<PaymentResponseDto>>.SuccessResponse(response, "Get Recent Payment Successfully"));
        }
    }
}
