namespace Backend_API.Models
{
    public class OrderMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid SenderId { get; set; }
        public User? Sender { get; set; }

        public Guid? SenderPageId { get; set; }
        public Page? SenderPage { get; set; }

        public string MessageText { get; set; } = string.Empty;

        public string? AttachmentUrl { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
