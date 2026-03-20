namespace Backend_API.Models.Enums
{
    public enum AddressPurpose
    {
        None = 0,
        Pickup = 1,
        Shipping = 2,
        Billing = 3
    }

    public enum ImageCategory
    {
        None = 0,
        Primary = 1,
        Secondary = 2,
        Matrix = 3
    }

    public enum IdentifierCategory
    {
        None = 0,
        Barcode = 1,
        MatrixRunout = 2,
        CatalogNumber = 3,
        Other = 4
    }

    public enum EntityStatus
    {
        Draft = 0,
        Active = 1,
        Hidden = 2,
        Blocked = 3
    }

    public enum AuditAction
    {
        None = 0,

        UserLogin = 100,
        UserRoleChanged = 101,
        AccountStatusChanged = 102,
        PageOwnershipClaimed = 110,

        CatalogStatusChanged = 200,
        ListingStatusChanged = 201,
        ExplicitFlagChanged = 202,
        OwnershipTransferred = 203,

        PriceChanged = 300,
        OrderCancelled = 301,
        OrderRefunded = 302,

        SystemConfigChanged = 400,
        TicketStatusChanged = 401
    }
}