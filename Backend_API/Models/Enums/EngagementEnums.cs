namespace Backend_API.Models.Enums
{
    public enum ReviewRole
    {
        None = 0,
        BuyerToSeller = 1,
        SellerToBuyer = 2
    }

    public enum TicketCategory
    {
        None = 0,
        PageVerification = 1,
        ReleaseOwnershipTransfer = 2,
        ContentViolation = 3,
        DataCorrection = 4,
        OrderDispute = 5,
        Other = 99
    }

    public enum TicketStatus
    {
        None = 0,
        Open = 1,
        InProgress = 2,
        WaitingForUser = 3,
        Resolved = 4,
        Closed = 5
    }

    public enum NotificationCategory
    {
        None = 0,

        AccountWarning = 101,
        AccountBanned = 102,
        PageVerified = 103,
        PageRejected = 104,
        SystemMaintenance = 199,

        NewOrderReceived = 201,
        OrderShipped = 202,
        OrderDelivered = 203,
        OrderCancelled = 204,
        PaymentFailed = 205,

        CatalogApproved = 301,
        CatalogRejected = 302,
        ListingBlocked = 303,
        DigitalFileReady = 304,

        NewComment = 401,
        NewReply = 402,
        NewReview = 403,

        TicketReplied = 501,
        TicketResolved = 502,
        TicketClosed = 503
    }
}