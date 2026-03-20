namespace Backend_API.Models.Relations
{
    public class MasterReleaseGenre
    {
        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public Guid GenreId { get; set; }
        public Genre? Genre { get; set; }
    }

    public class MasterReleaseStyle
    {
        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public Guid StyleId { get; set; }
        public Style? Style { get; set; }
    }

    public class MasterReleaseArtist
    {
        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public Guid ArtistPageId { get; set; }
        public Page? ArtistPage { get; set; }

        public bool IsHidden { get; set; } = false;
    }

    public class MasterReleaseLabel
    {
        public Guid MasterReleaseId { get; set; }
        public MasterRelease? MasterRelease { get; set; }

        public Guid LabelPageId { get; set; }
        public Page? LabelPage { get; set; }

        public bool IsHidden { get; set; } = true;
    }

    public class MasterTrackArtist
    {
        public Guid MasterTrackId { get; set; }
        public MasterTrack? MasterTrack { get; set; }

        public Guid ArtistPageId { get; set; }
        public Page? ArtistPage { get; set; }

        public string Role { get; set; } = "Featuring";
    }
}