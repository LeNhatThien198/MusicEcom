using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class Style
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<MasterReleaseStyle> MasterReleases { get; set; } = new List<MasterReleaseStyle>();
        public ICollection<ReleaseStyle> Releases { get; set; } = new List<ReleaseStyle>();
    }
}
