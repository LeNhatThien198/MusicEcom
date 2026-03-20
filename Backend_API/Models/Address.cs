using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public AddressPurpose Purpose { get; set; } = AddressPurpose.None;

        public string ContactName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string DetailedAddress { get; set; } = string.Empty;

        public string? CompanyName { get; set; }
        public string? TaxCode { get; set; }

        public bool IsDefault { get; set; } = false;

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public Guid? PageId { get; set; }
        public Page? Page { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
