using AutoMapper;
using rent_products_api.DataLayer.DTOs.Payments;
using rent_products_api.DataLayer.Models.Payments;
using rent_products_api.ServiceLayer.Utils;

namespace rent_products_api.DataLayer.Models.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Rent, PaymentDTO>()
                .ForMember(x => x.PaymentConfirmed, opt => opt.MapFrom(src => src.Payment.PaymentConfirmed))
                .ForMember(x => x.PaymentTime, opt => opt.MapFrom(src => GenericFunctions.ParseNullableDateTime(src.Payment.PaymentTime)))
                .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.RentedByUserId))
                .ForMember(x => x.PaymentStatus, opt => opt.MapFrom(src => src.Payment.PaymentStatus))
                .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Payment.Amount))
                .ForMember(x => x.UserPayingName, opt => opt.MapFrom(src => src.RentedByUser.LastName + " " + src.RentedByUser.FirstName));
        }
    }
}
