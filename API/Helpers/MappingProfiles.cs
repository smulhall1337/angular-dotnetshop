using API.Dto;
using AutoMapper;
using Core.Entities;

// to make use of AutoMapper and its functionality,
// we need to add it as a service in tthe Program.cs file
namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //for the dtos product brand,
            //map the productbrand.name to the product brand
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}
