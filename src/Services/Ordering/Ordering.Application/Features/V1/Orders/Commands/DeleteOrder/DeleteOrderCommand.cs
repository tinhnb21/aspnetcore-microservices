using MediatR;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<ApiResult<Order>>
    {
        public long Id { get; set; }

        public DeleteOrderCommand(long id)
        {
            Id = id;
        }
    }
}
