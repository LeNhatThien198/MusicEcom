using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public OrderItemCategory Category { get; set; } = OrderItemCategory.None;

        public Guid? ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid? TrackId { get; set; }
        public Track? Track { get; set; }

        public Guid? ListingId { get; set; }
        public Listing? Listing { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
