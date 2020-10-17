using System;
using AutoMapper;
using ContosoPets.Api.Models;

namespace ContosoPets.Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // 创建映射，从Product映射到ProductDto，默认是键名相同的映射
            // 如果键名不相同，则手动映射
            CreateMap<ProductAddDto, Product>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.ProductName));

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Name));
        }
    }
}
