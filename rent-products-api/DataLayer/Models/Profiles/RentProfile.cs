using AutoMapper;
using rent_products_api.DataLayer.DTOs.Email;
using rent_products_api.DataLayer.DTOs.Rents;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Profiles
{
    public class RentProfile:Profile
    {
        public RentProfile()
        {
            CreateMap<RentProductDTO, Rent>().ForMember(x=>x.RentedDate, opt=>opt.MapFrom(src=>GenericFunctions.ParseStringToDateTime(src.RentedDate)))
                .ForMember(x=>x.StartingHour, opt=>opt.MapFrom(src=>GenericFunctions.ParseStringToTimeSpan(src.StartingHour)))
                .ForMember(x => x.EndingHour, opt => opt.MapFrom(src => GenericFunctions.ParseStringToTimeSpan(src.EndingHour)))
                ;
            CreateMap<Rent, ProductRentedTime>().ForMember(x => x.RentedDate, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.RentedDate)))
                .ForMember(x => x.StartingHour, opt => opt.MapFrom(src => GenericFunctions.ParseTimeSpanToString(src.StartingHour)))
                .ForMember(x => x.EndingHour, opt => opt.MapFrom(src => GenericFunctions.ParseTimeSpanToString(src.EndingHour)));
            CreateMap<Rent, RentDTO>().ForMember(x => x.RentedDate, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.RentedDate)))
                .ForMember(x=>x.Start, opt=>opt.MapFrom(src=>src.StartingHour))
                .ForMember(x=>x.End, opt=>opt.MapFrom(src=>src.EndingHour))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(x=>x.UserId, opt=>opt.MapFrom(src=>src.RentedByUserId))
                .ForMember(x=>x.Name, opt=>opt.MapFrom(src=>src.RentedByUser!=null ? src.RentedByUser.LastName + " " + src.RentedByUser.FirstName : ""));
        }
    }
}
