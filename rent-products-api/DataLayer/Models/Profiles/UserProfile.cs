using AutoMapper;
using rent_products_api.DataLayer.DTOs.User;
using rent_products_api.Models;
using rent_products_api.Models.User;
using rent_products_api.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AdminUser, AdminUserDTO>().ForMember(x => x.Created, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.CreatedAt))); ;
            CreateMap<User, UserDTO>().ForMember(x => x.Created, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.CreatedAt))); ;
            CreateMap<RegisterUserDTO, User>();
            CreateMap<RegisterAdminUserDTO, AdminUser>();
            CreateMap<BaseUser, BaseUserDTO>().ForMember(x => x.Created, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.CreatedAt)))
                .ForMember(x => x.Updated, opt => opt.MapFrom(src => GenericFunctions.ParseNullableDateTime(src.Updated))) ;
            CreateMap<BaseUser, SimpleUserDTO>().ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.CreatedAt)))
                .ForMember(x => x.Updated, opt => opt.MapFrom(src => GenericFunctions.ParseNullableDateTime(src.Updated)));
                
            CreateMap<User, SimpleUserDTO>().ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => GenericFunctions.ParseDateTime(src.CreatedAt)))
                .ForMember(x => x.Updated, opt => opt.MapFrom(src => GenericFunctions.ParseNullableDateTime(src.Updated)));
            CreateMap<UpdateUserDTO, BaseUser>();
            CreateMap<UpdateUserDTO, User>();

            
        }
    }
}
