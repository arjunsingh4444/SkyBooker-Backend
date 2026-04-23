using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> FindByBookingIdAsync(string bookingId);
        Task<IList<Payment>> FindByUserIdAsync(string userId);
        Task<IList<Payment>> FindByStatusAsync(string status);
        Task<Payment?> FindByPaymentIdAsync(string paymentId);
        Task<Payment?> FindByTransactionIdAsync(string transactionId);
        Task<decimal> SumAmountByUserIdAsync(string userId);
        Task<IList<Payment>> FindByPaidAtBetweenAsync(DateTime startDate, DateTime endDate);
        Task<Payment> CreateAsync(Payment payment);
        Task UpdateAsync(Payment payment);
    }
}
