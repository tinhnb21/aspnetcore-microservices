﻿using Contracts.Common.Events;

namespace Ordering.Domain.OrderAggregate.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public long Id { get; private set; }
        public string UserName { get; private set; }
        public string DocumentNo { get; private set; }
        public string EmailAddress { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string ShippingAddress { get; private set; }
        public string InvoiceAddress { get; private set; }
        public string FullName { get; set; }

        public OrderCreatedEvent(long id, string userName, string documentNo, string emailAddress, decimal totalPrice, string shippingAddress, string invoiceAddress, string fullName)
        {
            Id = id;
            UserName = userName;
            DocumentNo = documentNo;
            EmailAddress = emailAddress;
            TotalPrice = totalPrice;
            ShippingAddress = shippingAddress;
            InvoiceAddress = invoiceAddress;
            FullName = fullName;
        }
    }
}
