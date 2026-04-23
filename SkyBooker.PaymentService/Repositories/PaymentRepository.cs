using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkyBooker.PaymentService.Data;
using SkyBooker.PaymentService.Entities;
using SkyBooker.PaymentService.Interfaces;

namespace SkyBooker.PaymentService.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> FindByBookingIdAsync(string bookingId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
        }

        public async Task<IList<Payment>> FindByUserIdAsync(string userId)
        {
            return await _context.Payments.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<IList<Payment>> FindByStatusAsync(string status)
        {
            return await _context.Payments.Where(p => p.Status == status).ToListAsync();
        }

        public async Task<Payment?> FindByPaymentIdAsync(string paymentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<Payment?> FindByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<decimal> SumAmountByUserIdAsync(string userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId && p.Status == "PAID")
                .SumAsync(p => p.Amount);
        }

        public async Task<IList<Payment>> FindByPaidAtBetweenAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate)
                .ToListAsync();
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
