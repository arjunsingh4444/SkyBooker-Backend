namespace SkyBooker.PaymentService.DTOs
{
    public class InitiatePaymentRequest
    {
        public string BookingId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
