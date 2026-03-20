using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class ListingIdentifier
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ListingId { get; set; }
        public Listing? Listing { get; set; }

        public IdentifierCategory Category { get; set; } = IdentifierCategory.None;
        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
