using EventBus.Messages.IntergrationEvents.Interfaces;

namespace EventBus.Messages.IntergrationEvents.Events
{
    public record BasketCheckoutEvent : IntergrationBaseEvent, IBasketCheckoutEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }
    }
}
