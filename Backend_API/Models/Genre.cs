using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class Genre
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<MasterReleaseGenre> MasterReleases { get; set; } = new List<MasterReleaseGenre>();
        public ICollection<ReleaseGenre> Releases { get; set; } = new List<ReleaseGenre>();
    }
}
