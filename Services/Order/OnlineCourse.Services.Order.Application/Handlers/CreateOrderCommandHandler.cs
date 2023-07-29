using MediatR;
using OnlineCourse.Services.Order.Application.Commands;
using OnlineCourse.Services.Order.Application.Dtos;
using OnlineCourse.Services.Order.Domain.OrderAggregate;
using OnlineCourse.Services.Order.Infrastructure;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Order.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
{
    private readonly OrderDbContext _context;

    public CreateOrderCommandHandler(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address
            (
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.ZipCode,
                request.Address.Line
            );

        Domain.OrderAggregate.Order newOrder = new(request.BuyerId, newAddress);

        request.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _context.Orders.AddAsync(newOrder, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Response<CreatedOrderDto>.Success(200, new CreatedOrderDto { OrderId = newOrder.Id });
    }
}