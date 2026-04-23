using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> InitiatePaymentAsync(string bookingId, decimal amount, string userId);
        Task<Payment> ProcessPaymentAsync(string payload, string signature);
        Task<Payment?> GetPaymentByBookingAsync(string bookingId);
        Task<IList<Payment>> GetPaymentsByUserAsync(string userId);
        Task<Payment> RefundPaymentAsync(string paymentId);
        Task<string> GetPaymentStatusAsync(string paymentId);
        Task UpdatePaymentStatusAsync(string paymentId, string status);
        Task<byte[]> GenerateReceiptAsync(string paymentId);
        Task<decimal> GetRevenueAsync(DateTime startDate, DateTime endDate);
    }
}
