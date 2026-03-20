using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class ReleaseImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public ImageCategory Category { get; set; } = ImageCategory.None;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
