using Backend_API.Models.Enums;
using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class Release
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PublicId { get; set; }

        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public string Title { get; set; } = string.Empty;
        public ReleaseEdition Edition { get; set; } = ReleaseEdition.None;
        public MediaFormat Format { get; set; } = MediaFormat.None;

        public int ReleaseYear { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Country { get; set; } = string.Empty;

        public Guid? OwnedByPageId { get; set; }
        public Page? OwnedByPage { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? DigitalPrice { get; set; }
        public decimal? DigitalDiscountPrice { get; set; }

        public int? StockQuantity { get; set; }
        public bool IsPreOrder { get; set; } = false;

        public string? Notes { get; set; }

        public bool IsSensitive { get; set; } = false;
        public bool IsExplicit { get; set; } = false;
        public EntityStatus Status { get; set; } = EntityStatus.Draft;

        public int TotalComments { get; set; } = 0;

        public float ListingRating { get; set; } = 0.0f;
        public int TotalListingReviews { get; set; } = 0;

        public Guid CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public Guid? CreatedByPageId { get; set; }
        public Page? CreatedByPage { get; set; }

        public Guid? LastUpdatedByUserId { get; set; }
        public User? LastUpdatedByUser { get; set; }
        public Guid? LastUpdatedByPageId { get; set; }
        public Page? LastUpdatedByPage { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<ReleaseGenre> Genres { get; set; } = new List<ReleaseGenre>();
        public ICollection<ReleaseStyle> Styles { get; set; } = new List<ReleaseStyle>();
        public ICollection<ReleaseArtist> Artists { get; set; } = new List<ReleaseArtist>();
        public ICollection<ReleaseLabel> Labels { get; set; } = new List<ReleaseLabel>();
        public ICollection<ReleaseImage> Images { get; set; } = new List<ReleaseImage>();
        public ICollection<ReleaseIdentifier> Identifiers { get; set; } = new List<ReleaseIdentifier>();
        public ICollection<Track> Tracks { get; set; } = new List<Track>();
        public ICollection<ReleaseComment> Comments { get; set; } = new List<ReleaseComment>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    }
}
