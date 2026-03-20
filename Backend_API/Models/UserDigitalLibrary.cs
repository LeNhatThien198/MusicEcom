using Backend_API.Models.Enums;

namespace Backend_API.Models
{
    public class UserDigitalLibrary
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Guid? ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid? TrackId { get; set; }
        public Track? Track { get; set; }

        public AcquisitionMethod Method { get; set; } = AcquisitionMethod.None;

        public bool IsUnlocked { get; set; } = true;

        public DateTimeOffset AcquiredAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
