using API.Dto.Store;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Address = Core.Entities.Identity.Address;

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
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap(); // dont need to do any extra config because our fields are an exact match
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(destination => destination.DeliveryMethod, 
                    option => option.MapFrom(source => source.DeliveryMethod.ShortName))
                .ForMember(destination => destination.ShippingPrice, 
                    option => option.MapFrom(source => source.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(destination => destination.ProductId, 
                    option => option.MapFrom(source => source.ItemOrdered.ProductItemId))
                .ForMember(destination => destination.PictureUrl, 
                    option => option.MapFrom(source => source.ItemOrdered.PictureUrl))
                .ForMember(destination => destination.ProductName, 
                    option => option.MapFrom(source => source.ItemOrdered.ProductName))
                .ForMember(destination => destination.PictureUrl, 
                    option => option.MapFrom<OrderItemUrlResolver>());
        }
    }
}
