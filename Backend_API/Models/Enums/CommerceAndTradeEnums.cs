namespace Backend_API.Models.Enums
{
    public enum ListingCondition
    {
        None = 0,
        Mint = 1,
        NearMint = 2,
        VeryGoodPlus = 3,
        VeryGood = 4,
        Good = 5,
        Fair = 6,
        Poor = 7
    }

    public enum OrderStatus
    {
        None = 0,
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Completed = 5,
        Cancelled = 6,
        Returned = 7
    }

    public enum PaymentStatus
    {
        None = 0,
        Unpaid = 1,
        Paid = 2,
        Failed = 3,
        Refunded = 4
    }

    public enum PaymentMethod
    {
        None = 0,
        COD = 1,
        VNPay = 2
    }

    public enum OrderItemCategory
    {
        None = 0,
        PrimaryPhysical = 1,
        PrimaryDigital = 2,
        SecondaryListing = 3
    }

    public enum AcquisitionMethod
    {
        None = 0,
        Purchased = 1,
        Redeemed = 2,
        Bundled = 3
    }

    public enum RedemptionStatus
    {
        None = 0,
        Active = 1,
        Redeemed = 2,
        Expired = 3
    }
}