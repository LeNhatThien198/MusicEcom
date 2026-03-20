using Backend_API.Models.Enums;

namespace Backend_API.Models.Relations
{
    public class PageUserRole
    {
        public Guid PageId { get; set; }
        public Page? Page { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public PageRole Role { get; set; }

        public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}