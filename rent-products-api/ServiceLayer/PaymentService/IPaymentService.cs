using rent_products_api.DataLayer.DTOs.Payments;
using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.PaymentService
{
    public interface IPaymentService
    {
        Task<ServiceResponse<List<PaymentDTO>>> GetPayments(Guid userId);
        Task<ServiceResponse<RequestPaymentDTO>> GoToPayment(GoToPaymentDTO paymentDTO, string origin);
        Task<ServiceResponse<object>> AddOrUpdatePayment(string env_key, string data);
    }
}
