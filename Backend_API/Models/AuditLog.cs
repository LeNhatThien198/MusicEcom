using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public AuditAction Action { get; set; } = AuditAction.None;

        public string EntityName { get; set; } = string.Empty;
        public string? EntityId { get; set; }

        public string? OldData { get; set; }
        public string? NewData { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
