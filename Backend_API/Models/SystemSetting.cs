namespace Backend_API.Models
{
    public class SystemSetting
    {
        public string Id { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid LastUpdatedByUserId { get; set; }
        public User? LastUpdatedByUser { get; set; }

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
