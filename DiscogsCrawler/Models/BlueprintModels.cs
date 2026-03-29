using Backend_API.Models.Enums; // Nhớ đảm bảo Enum khớp với Backend_API

namespace DiscogsCrawler.Models
{
    public class SeedingBlueprint
    {
        public List<BpUser> Users { get; set; } = new();
        public List<BpPage> Pages { get; set; } = new();
        public List<BpMasterRelease> MasterReleases { get; set; } = new();
        public List<BpRelease> Releases { get; set; } = new();
        public List<BpReleaseImage> ReleaseImages { get; set; } = new();
        public List<BpReleaseIdentifier> ReleaseIdentifiers { get; set; } = new();
        public List<BpTrack> Tracks { get; set; } = new();
    }

    public class MediaUploadPlan
    {
        public string LocalFilePath { get; set; } = string.Empty;
        public string TargetCloudinaryFolder { get; set; } = "MusicEcom_SeedData";
        public string ExpectedPublicId { get; set; } = string.Empty; 
        public string ResourceType { get; set; } = "image";
    }

    public class BpUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = "123456";
        public string FullName { get; set; } = string.Empty;
        public SystemRole SystemRole { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public bool IsEmailVerified { get; set; } = true;
        public bool IsTermsAccepted { get; set; } = true;
    }

    public class BpPage
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; 
        public PageCategory Category { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string OriginCountry { get; set; } = "International";
        public bool IsVerified { get; set; } = true;
        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public Guid CreatedByUserId { get; set; }
        public Guid OwnerUserId { get; set; }
        public Guid ManagerUserId { get; set; }
        public Guid? LastUpdatedByUserId { get; set; }
        public MediaUploadPlan? _AvatarUploadPlan { get; set; }
    }

    public class BpMasterRelease
    {
        public Guid Id { get; set; }
        public Guid ArtistPageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string CountryOfOrigin { get; set; } = "Unknown";
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        public Guid CreatedByUserId { get; set; }
        public Guid CreatedByPageId { get; set; } 
        public Guid? LastUpdatedByUserId { get; set; }
        public Guid? LastUpdatedByPageId { get; set; }
        public MediaUploadPlan? _CoverUploadPlan { get; set; }
    }

    public class BpRelease
    {
        public Guid Id { get; set; }
        public Guid MasterReleaseId { get; set; }
        public Guid OwnedByPageId { get; set; } 
        public Guid ArtistPageId { get; set; }  
        public string Title { get; set; } = string.Empty; 
        public ReleaseEdition Edition { get; set; } = ReleaseEdition.Original; 
        public MediaFormat Format { get; set; }
        public int ReleaseYear { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DigitalPrice { get; set; }
        public int StockQuantity { get; set; } = 10;
        public EntityStatus Status { get; set; } = EntityStatus.Active;
        public Guid CreatedByUserId { get; set; }
        public Guid CreatedByPageId { get; set; }
        public Guid? LastUpdatedByUserId { get; set; }
        public Guid? LastUpdatedByPageId { get; set; }
    }

    public class BpReleaseImage
    {
        public Guid Id { get; set; }
        public Guid ReleaseId { get; set; }
        public ImageCategory Category { get; set; }
        public MediaUploadPlan? _ImageUploadPlan { get; set; }
    }

    public class BpReleaseIdentifier 
    {
        public Guid Id { get; set; }
        public Guid ReleaseId { get; set; }
        public IdentifierCategory Category { get; set; } = IdentifierCategory.None;

        public string Description { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class BpTrack
    {
        public Guid Id { get; set; }
        public Guid ReleaseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty; // A1, B1...
        public int DurationSeconds { get; set; } = 0;
        public bool IsExplicit { get; set; } = false; // Bắt chữ _E
        public decimal Price { get; set; }
        public MediaUploadPlan? _AudioUploadPlan { get; set; }
    }
}