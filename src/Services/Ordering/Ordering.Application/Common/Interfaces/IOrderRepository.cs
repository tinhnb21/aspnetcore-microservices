﻿using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository
        : IRepositoryBase<Order, long>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
        Task<long> CreateOrder(Order order);
    }
}
