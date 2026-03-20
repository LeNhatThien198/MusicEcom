namespace Backend_API.Models
{
    public class ReleaseComment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid? PageId { get; set; }
        public Page? Page { get; set; }

        public string Content { get; set; } = string.Empty;

        public Guid? ParentCommentId { get; set; }
        public ReleaseComment? ParentComment { get; set; }

        public ICollection<ReleaseComment> Replies { get; set; } = new List<ReleaseComment>();

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
