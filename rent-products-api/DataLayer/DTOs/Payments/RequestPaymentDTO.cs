﻿using rent_products_api.DataLayer.Utils;

namespace rent_products_api.DataLayer.DTOs.Payments
{
    public class RequestPaymentDTO
    {
        public string Env_Key { get; set; }
        public string Data { get; set; }
        public string Url { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
