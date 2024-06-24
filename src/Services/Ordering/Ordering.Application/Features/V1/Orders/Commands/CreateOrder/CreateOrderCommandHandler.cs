using AutoMapper;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ISmtpEmailService _smtpEmailService;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger logger, IMapper mapper, ISmtpEmailService smtpEmailService)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(smtpEmailService));
        }

        private const string MethodName = "CreateOrderCommandHandler";

        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");
            var orderEntity = _mapper.Map<Order>(request);
            var addedOrder = await _orderRepository.CreateOrder(orderEntity);
            await _orderRepository.SaveChangesAsync();
            _logger.Information($"Order {orderEntity.Id} - Document No: {orderEntity.DocumentNo} was successfully created.");

            SendEmailAsync(addedOrder, cancellationToken);

            _logger.Information($"END: {MethodName} - Username: {request.UserName}");
            return new ApiSuccessResult<long>(addedOrder.Id);
        }

        private async Task SendEmailAsync(Order order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order was created",
                Subject = "Order was created"
            };

            try
            {
                await _smtpEmailService.SendEmailAsync(emailRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} failed due to an error with the email service: {ex.Message}");
            }
        }
    }
}
