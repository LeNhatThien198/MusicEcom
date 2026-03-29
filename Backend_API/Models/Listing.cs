using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class Listing
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PublicId { get; set; }

        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public string Title { get; set; } = string.Empty;

        public ListingCondition MediaCondition { get; set; } = ListingCondition.None;
        public ListingCondition SleeveCondition { get; set; } = ListingCondition.None;

        public string? Notes { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } = 1;

        public bool IsSensitive { get; set; } = false;

        public EntityStatus Status { get; set; } = EntityStatus.Draft;

        public Guid SellerId { get; set; }
        public User? Seller { get; set; }

        public Guid CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }
        public Guid? LastUpdatedByUserId { get; set; }
        public User? LastUpdatedByUser { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
        public ICollection<ListingIdentifier> Identifiers { get; set; } = new List<ListingIdentifier>();
        public ICollection<TransactionReview> Reviews { get; set; } = new List<TransactionReview>();
    }
}
