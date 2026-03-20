namespace Backend_API.Models.Enums
{
    public enum SystemRole
    {
        User = 0,
        Staff = 1,
        Admin = 2
    }

    public enum PageCategory
    {
        Artist = 0,
        Label = 1
    }

    public enum PageRole
    {
        Owner = 0,
        Manager = 1
    }

    public enum AccountStatus
    {
        Active = 0,
        Suspended = 1,
        Banned = 2
    }
}