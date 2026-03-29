using Backend_API.Models.Enums;
using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class MasterRelease
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int PublicId { get; set; }

        public string Title { get; set; } = string.Empty;
        public ReleaseCategory Category { get; set; } = ReleaseCategory.Album;
        public EntityStatus Status { get; set; } = EntityStatus.Draft;

        public int ReleaseYear { get; set; }
        public string CountryOfOrigin { get; set; } = string.Empty;

        public string CoverImageUrl { get; set; } = string.Empty;

        public bool IsSensitive { get; set; } = false;
        public bool IsExplicit { get; set; } = false;
        public int TotalReleaseComments { get; set; } = 0;

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

        public ICollection<MasterReleaseGenre> Genres { get; set; } = new List<MasterReleaseGenre>();
        public ICollection<MasterReleaseStyle> Styles { get; set; } = new List<MasterReleaseStyle>();

        public ICollection<MasterReleaseArtist> Artists { get; set; } = new List<MasterReleaseArtist>();
        public ICollection<MasterReleaseLabel> Labels { get; set; } = new List<MasterReleaseLabel>();
        public ICollection<Release> Releases { get; set; } = new List<Release>();
        public ICollection<MasterTrack> MasterTracks { get; set; } = new List<MasterTrack>();
    }
}
