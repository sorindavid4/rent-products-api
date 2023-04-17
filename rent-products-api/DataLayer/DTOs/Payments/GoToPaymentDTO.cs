using rent_products_api.DataLayer.Utils;
using System;

namespace rent_products_api.DataLayer.DTOs.Payments
{
    public class GoToPaymentDTO
    {
        public Guid UserId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
