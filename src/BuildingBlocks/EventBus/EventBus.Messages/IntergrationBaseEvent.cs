namespace EventBus.Messages
{
    public record IntergrationBaseEvent : IIntergrationEvent
    {
        public DateTime CreationDate { get; } = DateTime.UtcNow;
        public Guid Id { get; set; }
    }
}
