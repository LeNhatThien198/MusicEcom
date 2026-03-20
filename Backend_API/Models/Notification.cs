using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public Guid? PageId { get; set; }
        public Page? Page { get; set; }

        public NotificationCategory Category { get; set; } = NotificationCategory.None;

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public string? ActionUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
