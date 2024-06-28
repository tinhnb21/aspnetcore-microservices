using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger logger, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");
            var orderEntity = _mapper.Map<Order>(request);
            _orderRepository.Create(orderEntity);
            orderEntity.AddedOrder();
            await _orderRepository.SaveChangesAsync();
            _logger.Information($"Order {orderEntity.Id} - Document No: {orderEntity.DocumentNo} was successfully created.");
            _logger.Information($"END: {MethodName} - Username: {request.UserName}");
            return new ApiSuccessResult<long>(orderEntity.Id);
        }
    }
}
