using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class SupportTicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int TicketNumber { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TicketCategory Category { get; set; } = TicketCategory.None;
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public string? TargetEntityType { get; set; }
        public Guid? TargetEntityId { get; set; }

        public Guid CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public Guid? CreatedByPageId { get; set; }
        public Page? CreatedByPage { get; set; }

        public Guid? LastUpdatedByUserId { get; set; }
        public User? LastUpdatedByUser { get; set; }
        public Guid? LastUpdatedByPageId { get; set; }
        public Page? LastUpdatedByPage { get; set; }

        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }
}
