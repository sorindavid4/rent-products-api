using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.ServiceLayer.RentService
{
    public interface IRentService
    {
        Task<ServiceResponse<List<RentDTO>>> GetMyRents(Guid userId);
        Task<ServiceResponse<Object>> RentProduct(RentProductDTO rentDTO);
        Task<ServiceResponse<List<ProductRentedTime>>> GetProductRentedTimes(Guid productId);
        Task<ServiceResponse<Object>> ConfirmRent(Guid rentId);
        Task<ServiceResponse<Object>> UnconfirmRent(Guid rentId);
        Task<ServiceResponse<Object>> ConfirmRentPayment(Guid rentId);
        Task<ServiceResponse<Object>> CancelRent(Guid rentId);
        Task<ServiceResponse<Object>> UncancelRent(Guid rentId);

    }
}
