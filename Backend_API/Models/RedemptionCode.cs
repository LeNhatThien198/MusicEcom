using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class RedemptionCode
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Code { get; set; } = string.Empty;

        public Guid? ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid? TrackId { get; set; }
        public Track? Track { get; set; }

        public RedemptionStatus Status { get; set; } = RedemptionStatus.Active;

        public int MaxUses { get; set; } = 1;
        public int CurrentUses { get; set; } = 0;

        public DateTimeOffset? ExpiresAt { get; set; }

        public Guid CreatedByPageId { get; set; }
        public Page? CreatedByPage { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
