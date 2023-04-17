using AutoMapper;
using Microsoft.AspNetCore.Http;
using rent_products_api.DataLayer.DTOs.Product;
using rent_products_api.DataLayer.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Models.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductDTO, rent_products_api.DataLayer.Models.Product.Product>();
            CreateMap<rent_products_api.DataLayer.Models.Product.Product, ProductDTO>().ForMember(x=>x.ImageUrls, opt=>opt.MapFrom(src=>src.Images.Select(a=>a.ImagePath).ToList()));
            CreateMap<IFormFile, ProductImage>().ForMember(x => x.ImageExtension, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(x => x.ImageTitle, opt => opt.MapFrom(src => src.FileName));
        }
    }
}
