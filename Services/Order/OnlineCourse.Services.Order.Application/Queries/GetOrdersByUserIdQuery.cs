using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OnlineCourse.Services.Order.Application.Dtos;
using OnlineCourse.Shared.Dtos;

namespace OnlineCourse.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery:IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
