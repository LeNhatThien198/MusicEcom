using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class MasterTrack
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
        public bool IsExplicit { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<MasterTrackArtist> Artists { get; set; } = new List<MasterTrackArtist>();
        public ICollection<Track> Tracks { get; set; } = new List<Track>();
    }
}
