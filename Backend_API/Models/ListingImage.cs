using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class ListingImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ListingId { get; set; }
        public Listing? Listing { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
        public ImageCategory Category { get; set; } = ImageCategory.None;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
