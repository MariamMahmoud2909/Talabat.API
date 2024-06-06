using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Identity;

namespace Talabat.APIs.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
				.ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
				.ForMember(D => D.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<Core.Identity.Address, AddressDto>().ReverseMap();
			CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();

			CreateMap<Order, OrderToReturnDto>()
				.ForMember(D => D.DeliveryMethod , O => O.MapFrom(S => S.DeliveryMethod.ShortName))
				.ForMember(D => D.DeliveryMethodCost , O => O.MapFrom(S => S.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
				.ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
				.ForMember(D => D.PictureUrl , O => O.MapFrom(S => S.Product.PictureUrl))
				.ForMember(D => D.PictureUrl , O => O.MapFrom<OrderItemPictureUrlResolver>());

		}
	}
}
