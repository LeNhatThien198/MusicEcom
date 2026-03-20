using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class Track
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid? MasterTrackId { get; set; }
        public MasterTrack? MasterTrack { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }

        public decimal? Price { get; set; }

        public string? PreviewMp3Url { get; set; }
        public string? OriginalFlacUrl { get; set; }

        public bool IsExplicit { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<TrackArtist> Artists { get; set; } = new List<TrackArtist>();
    }
}
