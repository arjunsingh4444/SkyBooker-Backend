using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.PaymentService.Interfaces;
using SkyBooker.PaymentService.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SkyBooker.PaymentService.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initiate")]
        [Authorize]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
        {
            var payment = await _paymentService.InitiatePaymentAsync(request.BookingId, request.Amount, request.UserId);
            return Ok(payment);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ProcessWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync();
            var signature = Request.Headers["X-Razorpay-Signature"].ToString();
            
            var payment = await _paymentService.ProcessPaymentAsync(payload, signature ?? string.Empty);
            if (payment == null) return BadRequest("Invalid payload or signature");
            
            return Ok(new { status = "success" });
        }

        [HttpPost("{paymentId}/refund")]
        [Authorize(Roles = "Passenger,Admin")]
        public async Task<IActionResult> RefundPayment(string paymentId)
        {
            var payment = await _paymentService.RefundPaymentAsync(paymentId);
            return Ok(payment);
        }

        [HttpGet("booking/{bookingId}")]
        [Authorize]
        public async Task<IActionResult> GetByBooking(string bookingId)
        {
            var payment = await _paymentService.GetPaymentByBookingAsync(bookingId);
            if (payment == null) return NotFound();
            return Ok(payment);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var payments = await _paymentService.GetPaymentsByUserAsync(userId);
            return Ok(payments);
        }

        [HttpGet("{paymentId}/status")]
        [Authorize]
        public async Task<IActionResult> GetStatus(string paymentId)
        {
            var status = await _paymentService.GetPaymentStatusAsync(paymentId);
            return Ok(new { paymentId, status });
        }

        [HttpGet("{paymentId}/receipt")]
        [Authorize]
        public async Task<IActionResult> GenerateReceipt(string paymentId)
        {
            try
            {
                var pdfBytes = await _paymentService.GenerateReceiptAsync(paymentId);
                return File(pdfBytes, "application/pdf", $"Receipt_{paymentId}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("revenue")]
        [Authorize(Roles = "Airline Staff,Admin")]
        public async Task<IActionResult> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenue = await _paymentService.GetRevenueAsync(startDate, endDate);
            return Ok(new { startDate, endDate, revenue });
        }
    }
}
