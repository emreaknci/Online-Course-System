using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Services.Order.Application.Commands;
using OnlineCourse.Services.Order.Application.Queries;
using OnlineCourse.Shared.ControllerBase;
using OnlineCourse.Shared.Services;

namespace OnlineCourse.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = _sharedIdentityService.GetUserId;
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = userId});

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator.Send(createOrderCommand);

            return CreateActionResultInstance(response);
        }
    }
}
