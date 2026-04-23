using System;
using System.ComponentModel.DataAnnotations;

namespace SkyBooker.PaymentService.Entities
{
    public class Payment
    {
        [Key]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string BookingId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "INR";

        [Required]
        public string Status { get; set; } = "PENDING"; // PENDING, PAID, FAILED, REFUNDED

        public string? PaymentMode { get; set; } // CARD, UPI, NETBANKING, WALLET

        public string? TransactionId { get; set; }

        public string? GatewayResponse { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? RefundedAt { get; set; }

        public decimal RefundAmount { get; set; } = 0;
    }
}
