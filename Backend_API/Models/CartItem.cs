using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }

        public OrderItemCategory Category { get; set; } = OrderItemCategory.None;

        public Guid? ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid? TrackId { get; set; }
        public Track? Track { get; set; }

        public Guid? ListingId { get; set; }
        public Listing? Listing { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
