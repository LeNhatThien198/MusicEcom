using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid BuyerUserId { get; set; }
        public User? BuyerUser { get; set; }

        public Guid? SellerUserId { get; set; }
        public User? SellerUser { get; set; }

        public Guid? SellerPageId { get; set; }
        public Page? SellerPage { get; set; }

        public string ShippingContactName { get; set; } = string.Empty;
        public string ShippingPhoneNumber { get; set; } = string.Empty;
        public string ShippingFullAddress { get; set; } = string.Empty;

        public string? BillingCompanyName { get; set; }
        public string? BillingTaxCode { get; set; }
        public string? BillingFullAddress { get; set; }

        public decimal ShippingFee { get; set; } = 0m;
        public string? TrackingCode { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.None;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<OrderMessage> Messages { get; set; } = new List<OrderMessage>();
        public ICollection<TransactionReview> Reviews { get; set; } = new List<TransactionReview>();
    }
}
