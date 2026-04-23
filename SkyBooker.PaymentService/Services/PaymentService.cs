using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using SkyBooker.PaymentService.Entities;
using SkyBooker.PaymentService.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SkyBooker.PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _configuration;
        private readonly RazorpayClient? _razorpayClient;

        public PaymentService(IPaymentRepository paymentRepository, IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _configuration = configuration;
            
            var keyId = _configuration["Razorpay:KeyId"];
            var keySecret = _configuration["Razorpay:KeySecret"];
            if (!string.IsNullOrEmpty(keyId) && !string.IsNullOrEmpty(keySecret))
            {
                _razorpayClient = new RazorpayClient(keyId, keySecret);
            }
        }

        public async Task<Entities.Payment> InitiatePaymentAsync(string bookingId, decimal amount, string userId)
        {
            var options = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) }, // amount in the smallest currency unit
                { "currency", "INR" },
                { "receipt", bookingId }
            };

            Entities.Payment payment = new Entities.Payment
            {
                BookingId = bookingId,
                UserId = userId,
                Amount = amount,
                Status = "PENDING"
            };

            if (_razorpayClient != null)
            {
                try
                {
                    Order order = _razorpayClient.Order.Create(options);
                    payment.TransactionId = order["id"].ToString();
                }
                catch (Exception ex)
                {
                    payment.GatewayResponse = ex.Message;
                    payment.Status = "FAILED";
                }
            }
            else
            {
                payment.TransactionId = "TEST_TXN_" + Guid.NewGuid().ToString().Substring(0, 8);
            }

            return await _paymentRepository.CreateAsync(payment);
        }

        public async Task<Entities.Payment> ProcessPaymentAsync(string payload, string signature)
        {
            // Webhook payload processing and HMAC verification
            // In a real scenario, use Razorpay's Utils.verifyWebhookSignature
            // For this implementation, we will simulate the extraction
            // assuming payload has transaction id and status
            
            // Simulation of finding a payment by transaction ID
            // Let's assume transactionId is extracted from payload
            var transactionId = "extracted_from_payload"; // Placeholder
            
            var payment = await _paymentRepository.FindByTransactionIdAsync(transactionId);
            if (payment != null)
            {
                payment.Status = "PAID";
                payment.PaidAt = DateTime.UtcNow;
                payment.GatewayResponse = payload;
                await _paymentRepository.UpdateAsync(payment);
            }
            
            return payment;
        }

        public async Task<Entities.Payment?> GetPaymentByBookingAsync(string bookingId)
        {
            return await _paymentRepository.FindByBookingIdAsync(bookingId);
        }

        public async Task<IList<Entities.Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await _paymentRepository.FindByUserIdAsync(userId);
        }

        public async Task<Entities.Payment> RefundPaymentAsync(string paymentId)
        {
            var payment = await _paymentRepository.FindByPaymentIdAsync(paymentId);
            if (payment == null) throw new Exception("Payment not found");

            if (payment.Status == "PAID" && _razorpayClient != null && !string.IsNullOrEmpty(payment.TransactionId))
            {
                try
                {
                    var options = new Dictionary<string, object>
                    {
                        { "amount", (int)(payment.Amount * 100) }
                    };
                    Refund refund = _razorpayClient.Refund.Create(options);
                    payment.Status = "REFUNDED";
                    payment.RefundedAt = DateTime.UtcNow;
                    payment.RefundAmount = payment.Amount;
                }
                catch (Exception ex)
                {
                    payment.GatewayResponse = "Refund failed: " + ex.Message;
                }
            }
            else
            {
                // Test fallback
                payment.Status = "REFUNDED";
                payment.RefundedAt = DateTime.UtcNow;
                payment.RefundAmount = payment.Amount;
            }

            await _paymentRepository.UpdateAsync(payment);
            return payment;
        }

        public async Task<string> GetPaymentStatusAsync(string paymentId)
        {
            var payment = await _paymentRepository.FindByPaymentIdAsync(paymentId);
            return payment?.Status ?? "NOT_FOUND";
        }

        public async Task UpdatePaymentStatusAsync(string paymentId, string status)
        {
            var payment = await _paymentRepository.FindByPaymentIdAsync(paymentId);
            if (payment != null)
            {
                payment.Status = status;
                await _paymentRepository.UpdateAsync(payment);
            }
        }

        public async Task<byte[]> GenerateReceiptAsync(string paymentId)
        {
            var payment = await _paymentRepository.FindByPaymentIdAsync(paymentId);
            if (payment == null) throw new Exception("Payment not found");

            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    
                    page.Header().Text("SkyBooker Payment Receipt")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Darken2);
                    
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                            x.Item().Text($"Receipt for Booking ID: {payment.BookingId}");
                            x.Item().Text($"Payment ID: {payment.PaymentId}");
                            x.Item().Text($"Amount: {payment.Currency} {payment.Amount}");
                            x.Item().Text($"Status: {payment.Status}");
                            if (payment.PaidAt.HasValue)
                                x.Item().Text($"Paid At: {payment.PaidAt.Value:f}");
                            x.Item().Text($"Transaction ID: {payment.TransactionId}");
                        });
                    
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<decimal> GetRevenueAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _paymentRepository.FindByPaidAtBetweenAsync(startDate, endDate);
            return payments.Where(p => p.Status == "PAID").Sum(p => p.Amount);
        }
    }
}
