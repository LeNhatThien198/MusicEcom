using Backend_API.Models.Enums;
using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Email { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;

        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; } 
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? SellerTerms { get; set; }

        public bool IsTermsAccepted { get; set; } = false;

        public SystemRole SystemRole { get; set; } = SystemRole.User;
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public float SellerRating { get; set; } = 0.0f;
        public int TotalSellerReviews { get; set; } = 0;

        public float BuyerRating { get; set; } = 0.0f;
        public int TotalBuyerReviews { get; set; } = 0;

        public Cart? Cart { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<PageUserRole> PageRoles { get; set; } = new List<PageUserRole>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
        public ICollection<TransactionReview> WrittenReviews { get; set; } = new List<TransactionReview>();
        public ICollection<TransactionReview> ReceivedReviews { get; set; } = new List<TransactionReview>();
        public ICollection<ReleaseComment> Comments { get; set; } = new List<ReleaseComment>();
        public ICollection<UserDigitalLibrary> DigitalLibraries { get; set; } = new List<UserDigitalLibrary>();
        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
