using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Identity;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			//for member : destination , mapfrom : source
			CreateMap<Product, ProductToReturnDto>().ForMember(p => p.Brand, O => O.MapFrom(s => s.Brand.Name));
			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();
        }
	}
}