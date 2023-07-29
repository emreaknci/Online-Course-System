using AutoMapper;
using OnlineCourse.Services.Order.Application.Dtos;
using OnlineCourse.Services.Order.Domain.OrderAggregate;

namespace OnlineCourse.Services.Order.Application.Mapping;

public class CustomMapping : Profile
{
    public CustomMapping()
    {
        CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}