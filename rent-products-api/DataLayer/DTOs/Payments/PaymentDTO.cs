using System;

namespace rent_products_api.DataLayer.DTOs.Payments
{
    public class PaymentDTO
    {
        public Guid PaymentId { get; set; }
        public Guid UserId { get; set; }
        public Guid RentId { get; set; }
        public string PaymentTime { get; set; }
        public string PaymentStatus { get; set; }
        public string UserPayingName { get; set; }
        public string Amount { get; set; }
        public bool PaymentConfirmed { get; set; }
    }
}
