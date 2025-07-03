using HomeHeroSystem.Services.Interfaces;
using HomeHeroSystem.Services.Models.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeHeroSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new CreatePaymentResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid input data",
                        Data = null
                    });
                }

                var result = await _paymentService.CreatePaymentAsync(request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CreatePaymentResponse
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred while creating payment",
                    Data = null
                });
            }
        }

        [HttpPut("{paymentId}/status")]
        public async Task<IActionResult> UpdatePaymentStatusAsync(int paymentId, [FromBody] UpdatePaymentStatusRequest request)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return BadRequest(new UpdatePaymentStatusResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid payment ID",
                        Data = null
                    });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new UpdatePaymentStatusResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid input data",
                        Data = null
                    });
                }

                var result = await _paymentService.UpdatePaymentStatusAsync(paymentId, request);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UpdatePaymentStatusResponse
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred while updating payment status",
                    Data = null
                });
            }
        }
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                if (paymentId <= 0)
                {
                    return BadRequest(new
                    {
                        IsSuccess = false,
                        Message = "Invalid payment ID",
                        Data = (object)null
                    });
                }

                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);

                if (payment == null)
                {
                    return NotFound(new
                    {
                        IsSuccess = false,
                        Message = "Payment not found",
                        Data = (object)null
                    });
                }

                return Ok(new
                {
                    IsSuccess = true,
                    Message = "Payment retrieved successfully",
                    Data = payment
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    IsSuccess = false,
                    Message = "Internal server error occurred while retrieving payment",
                    Data = (object)null
                });
            }
        }
    }
}
