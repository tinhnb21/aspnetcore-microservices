using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity == null) throw new NotFoundException(nameof(Order), request.Id);
            _orderRepository.Delete(orderEntity);
            orderEntity.DeletedOrder();
            await _orderRepository.SaveChangesAsync();

            _logger.Information($"Order {orderEntity.Id} was successfully deleted.");

            return Unit.Value;
        }
    }
}
