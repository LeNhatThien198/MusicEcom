using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class TransactionReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid? ListingId { get; set; }
        public Listing? Listing { get; set; }

        public Guid AuthorUserId { get; set; }
        public User? AuthorUser { get; set; }

        public Guid? AuthorPageId { get; set; }
        public Page? AuthorPage { get; set; }

        public Guid TargetUserId { get; set; }
        public User? TargetUser { get; set; }


        public ReviewRole Role { get; set; } = ReviewRole.None;

        public int Rating { get; set; }
        public string? Content { get; set; }
        public string? ReplyContent { get; set; }
        public DateTimeOffset? RepliedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
