using Backend_API.Models.Enums;

namespace Backend_API.Models.Relations
{
    public class ReleaseGenre
    {
        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }
    }

    public class ReleaseStyle
    {
        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid StyleId { get; set; }
        public Style? Style { get; set; }
    }

    public class ReleaseArtist
    {
        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid ArtistPageId { get; set; }
        public Page? ArtistPage { get; set; }

        public CollaborationRole Role { get; set; } = CollaborationRole.CreditOnly;
    }

    public class ReleaseLabel
    {
        public Guid ReleaseId { get; set; }
        public Release? Release { get; set; }

        public Guid LabelPageId { get; set; }
        public Page? LabelPage { get; set; }

        public CollaborationRole Role { get; set; } = CollaborationRole.CreditOnly;
    }

    public class TrackArtist
    {
        public Guid TrackId { get; set; }
        public Track? Track { get; set; }

        public Guid ArtistPageId { get; set; }
        public Page? ArtistPage { get; set; }

        public string Role { get; set; } = "Featuring";
    }
}