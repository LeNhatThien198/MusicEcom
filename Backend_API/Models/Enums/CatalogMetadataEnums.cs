namespace Backend_API.Models.Enums
{
    public enum ReleaseCategory
    {
        Album = 0,
        Single = 1,
        EP = 2,
        Mixtape = 3
    }

    public enum ReleaseEdition
    {
        None = 0,
        Original = 1,
        Repress = 2,
        Reissue = 3
    }

    public enum MediaFormat
    {
        None = 0,
        Vinyl = 1,
        CD = 2,
        Cassette = 3
    }

    public enum CollaborationRole
    {
        CreditOnly = 0,
        CoManager = 1
    }
}