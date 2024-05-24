namespace Ordering.Domain.Enums
{
    public enum EOrderStatus
    {
        New = 1, //start with 1, 0 is used for fillter All = 0
        Pending, //order ispending, not any activities for a period time.
        Paid, //order is paid
        Shipping, //order is on the shipping
        Fulfilled, //order is fulfilled
    }
}
