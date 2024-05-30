using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<Order>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResult<Order>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var findById = await _orderRepository.GetByIdAsync(request.Id);
            if (findById == null) throw new Exception();
            await _orderRepository.DeleteAsync(findById);
            await _orderRepository.SaveChangesAsync();
            return new ApiSuccessResult<Order>(findById);
        }
    }
}
