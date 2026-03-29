using Backend_API.Models.Enums;
using Backend_API.Models.Relations;

namespace Backend_API.Models
{
    public class Page
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Slug { get; set; } = string.Empty;
        public PageCategory Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string OriginCountry { get; set; } = string.Empty;
        public string? SellerTerms { get; set; }

        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? WebsiteUrl { get; set; }

        public string? AvatarUrl { get; set; }
        public string? CoverUrl { get; set; }

        public bool IsVerified { get; set; } = false;
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public int TotalReleaseComments { get; set; } = 0;

        public Guid CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }
        public Guid? LastUpdatedByUserId { get; set; }
        public User? LastUpdatedByUser { get; set; }


        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<PageUserRole> UserRoles { get; set; } = new List<PageUserRole>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Release> OwnedReleases { get; set; } = new List<Release>();
        public ICollection<ReleaseComment> Comments { get; set; } = new List<ReleaseComment>();
        public ICollection<RedemptionCode> RedemptionCodes { get; set; } = new List<RedemptionCode>();
        public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
