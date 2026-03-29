using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SellerTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SystemRole = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SellerRating = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    TotalSellerReviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BuyerRating = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    TotalBuyerReviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SellerTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CoverUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalReleaseComments = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pages_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSettings_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Purpose = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DetailedAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.CheckConstraint("CK_Address_Owner", "([UserId] IS NOT NULL AND [PageId] IS NULL) OR ([UserId] IS NULL AND [PageId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Addresses_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterReleases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsExplicit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TotalReleaseComments = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ListingRating = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    TotalListingReviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReleases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterReleases_Pages_CreatedByPageId",
                        column: x => x.CreatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterReleases_Pages_LastUpdatedByPageId",
                        column: x => x.LastUpdatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterReleases_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterReleases_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ActionUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.CheckConstraint("CK_Notification_UserOrPage", "([UserId] IS NOT NULL AND [PageId] IS NULL) OR ([UserId] IS NULL AND [PageId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Notifications_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentSessionId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BuyerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SellerPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShippingPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShippingFullAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BillingCompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BillingTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BillingFullAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TrackingCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Order_SellerType", "([SellerUserId] IS NOT NULL AND [SellerPageId] IS NULL) OR ([SellerUserId] IS NULL AND [SellerPageId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Orders_Pages_LastUpdatedByPageId",
                        column: x => x.LastUpdatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Pages_SellerPageId",
                        column: x => x.SellerPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_BuyerUserId",
                        column: x => x.BuyerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_SellerUserId",
                        column: x => x.SellerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageUserRoles",
                columns: table => new
                {
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageUserRoles", x => new { x.PageId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PageUserRoles_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TargetEntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Pages_CreatedByPageId",
                        column: x => x.CreatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Pages_LastUpdatedByPageId",
                        column: x => x.LastUpdatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Users_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportTickets_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterReleaseArtists",
                columns: table => new
                {
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReleaseArtists", x => new { x.MasterReleaseId, x.ArtistPageId });
                    table.ForeignKey(
                        name: "FK_MasterReleaseArtists_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterReleaseArtists_Pages_ArtistPageId",
                        column: x => x.ArtistPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterReleaseGenres",
                columns: table => new
                {
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReleaseGenres", x => new { x.MasterReleaseId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MasterReleaseGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterReleaseGenres_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterReleaseLabels",
                columns: table => new
                {
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabelPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReleaseLabels", x => new { x.MasterReleaseId, x.LabelPageId });
                    table.ForeignKey(
                        name: "FK_MasterReleaseLabels_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterReleaseLabels_Pages_LabelPageId",
                        column: x => x.LabelPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterReleaseStyles",
                columns: table => new
                {
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StyleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterReleaseStyles", x => new { x.MasterReleaseId, x.StyleId });
                    table.ForeignKey(
                        name: "FK_MasterReleaseStyles_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterReleaseStyles_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterTracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    IsExplicit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTracks_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Edition = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Format = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReleaseYear = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OwnedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DigitalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DigitalDiscountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    IsPreOrder = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsExplicit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalComments = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ListingRating = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    TotalListingReviews = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastUpdatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Releases_MasterReleases_MasterReleaseId",
                        column: x => x.MasterReleaseId,
                        principalTable: "MasterReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Releases_Pages_CreatedByPageId",
                        column: x => x.CreatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Releases_Pages_LastUpdatedByPageId",
                        column: x => x.LastUpdatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Releases_Pages_OwnedByPageId",
                        column: x => x.OwnedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Releases_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Releases_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderMessages_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderMessages_Pages_SenderPageId",
                        column: x => x.SenderPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Pages_SenderPageId",
                        column: x => x.SenderPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketMessages_SupportTickets_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "SupportTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterTrackArtists",
                columns: table => new
                {
                    MasterTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Featuring")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTrackArtists", x => new { x.MasterTrackId, x.ArtistPageId });
                    table.ForeignKey(
                        name: "FK_MasterTrackArtists_MasterTracks_MasterTrackId",
                        column: x => x.MasterTrackId,
                        principalTable: "MasterTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterTrackArtists_Pages_ArtistPageId",
                        column: x => x.ArtistPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MediaCondition = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SleeveCondition = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Listings_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Listings_Users_LastUpdatedByUserId",
                        column: x => x.LastUpdatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Listings_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseArtists",
                columns: table => new
                {
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseArtists", x => new { x.ReleaseId, x.ArtistPageId });
                    table.ForeignKey(
                        name: "FK_ReleaseArtists_Pages_ArtistPageId",
                        column: x => x.ArtistPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseArtists_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseComments_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseComments_ReleaseComments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "ReleaseComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReleaseComments_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseGenres",
                columns: table => new
                {
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseGenres", x => new { x.ReleaseId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ReleaseGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseGenres_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseIdentifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseIdentifiers_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseImages_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseLabels",
                columns: table => new
                {
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LabelPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseLabels", x => new { x.ReleaseId, x.LabelPageId });
                    table.ForeignKey(
                        name: "FK_ReleaseLabels_Pages_LabelPageId",
                        column: x => x.LabelPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReleaseLabels_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseStyles",
                columns: table => new
                {
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StyleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseStyles", x => new { x.ReleaseId, x.StyleId });
                    table.ForeignKey(
                        name: "FK_ReleaseStyles_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseStyles_Styles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MasterTrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreviewMp3Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    OriginalFlacUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    IsExplicit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_MasterTracks_MasterTrackId",
                        column: x => x.MasterTrackId,
                        principalTable: "MasterTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tracks_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingIdentifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingIdentifiers_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListingImages_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReplyContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RepliedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionReviews", x => x.Id);
                    table.CheckConstraint("CK_TransactionReview_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_TransactionReviews_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionReviews_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionReviews_Pages_AuthorPageId",
                        column: x => x.AuthorPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionReviews_Users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionReviews_Users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.CheckConstraint("CK_CartItem_ItemType", "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL AND [ListingId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL AND [ListingId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NOT NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NULL)");
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.CheckConstraint("CK_OrderItem_ItemType", "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL AND [ListingId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL AND [ListingId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_OrderItems_Listings_ListingId",
                        column: x => x.ListingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RedemptionCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MaxUses = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CurrentUses = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedemptionCodes", x => x.Id);
                    table.CheckConstraint("CK_RedemptionCode_ItemType", "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_RedemptionCodes_Pages_CreatedByPageId",
                        column: x => x.CreatedByPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RedemptionCodes_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RedemptionCodes_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RedemptionCodes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Featuring")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackArtists", x => new { x.TrackId, x.ArtistPageId });
                    table.ForeignKey(
                        name: "FK_TrackArtists_Pages_ArtistPageId",
                        column: x => x.ArtistPageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackArtists_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDigitalLibraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Method = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsUnlocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AcquiredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDigitalLibraries", x => x.Id);
                    table.CheckConstraint("CK_UserDigitalLibrary_ItemType", "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_UserDigitalLibraries_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDigitalLibraries_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDigitalLibraries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PageId",
                table: "Addresses",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ListingId",
                table: "CartItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ReleaseId",
                table: "CartItems",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_TrackId",
                table: "CartItems",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ListingIdentifiers_ListingId",
                table: "ListingIdentifiers",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_ListingId",
                table: "ListingImages",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CreatedByUserId",
                table: "Listings",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_DeletedAt",
                table: "Listings",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_LastUpdatedByUserId",
                table: "Listings",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_PublicId",
                table: "Listings",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ReleaseId",
                table: "Listings",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_SellerId",
                table: "Listings",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleaseArtists_ArtistPageId",
                table: "MasterReleaseArtists",
                column: "ArtistPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleaseGenres_GenreId",
                table: "MasterReleaseGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleaseLabels_LabelPageId",
                table: "MasterReleaseLabels",
                column: "LabelPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_CreatedByPageId",
                table: "MasterReleases",
                column: "CreatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_CreatedByUserId",
                table: "MasterReleases",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_DeletedAt",
                table: "MasterReleases",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_LastUpdatedByPageId",
                table: "MasterReleases",
                column: "LastUpdatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_LastUpdatedByUserId",
                table: "MasterReleases",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleases_PublicId",
                table: "MasterReleases",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterReleaseStyles_StyleId",
                table: "MasterReleaseStyles",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTrackArtists_ArtistPageId",
                table: "MasterTrackArtists",
                column: "ArtistPageId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTracks_MasterReleaseId",
                table: "MasterTracks",
                column: "MasterReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PageId",
                table: "Notifications",
                column: "PageId",
                filter: "[IsRead] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId",
                filter: "[IsRead] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ListingId",
                table: "OrderItems",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ReleaseId",
                table: "OrderItems",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_TrackId",
                table: "OrderItems",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMessages_OrderId",
                table: "OrderMessages",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMessages_SenderId",
                table: "OrderMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMessages_SenderPageId",
                table: "OrderMessages",
                column: "SenderPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerUserId",
                table: "Orders",
                column: "BuyerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LastUpdatedByPageId",
                table: "Orders",
                column: "LastUpdatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_LastUpdatedByUserId",
                table: "Orders",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderCode",
                table: "Orders",
                column: "OrderCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerPageId",
                table: "Orders",
                column: "SellerPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerUserId",
                table: "Orders",
                column: "SellerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CreatedByUserId",
                table: "Pages",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_DeletedAt",
                table: "Pages",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_LastUpdatedByUserId",
                table: "Pages",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Slug",
                table: "Pages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageUserRoles_UserId",
                table: "PageUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionCodes_Code",
                table: "RedemptionCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionCodes_CreatedByPageId",
                table: "RedemptionCodes",
                column: "CreatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionCodes_CreatedByUserId",
                table: "RedemptionCodes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionCodes_ReleaseId",
                table: "RedemptionCodes",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionCodes_TrackId",
                table: "RedemptionCodes",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseArtists_ArtistPageId",
                table: "ReleaseArtists",
                column: "ArtistPageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComments_PageId",
                table: "ReleaseComments",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComments_ParentCommentId",
                table: "ReleaseComments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComments_ReleaseId",
                table: "ReleaseComments",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseComments_UserId",
                table: "ReleaseComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseGenres_GenreId",
                table: "ReleaseGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseIdentifiers_ReleaseId",
                table: "ReleaseIdentifiers",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseImages_ReleaseId_SortOrder",
                table: "ReleaseImages",
                columns: new[] { "ReleaseId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseLabels_LabelPageId",
                table: "ReleaseLabels",
                column: "LabelPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_CreatedByPageId",
                table: "Releases",
                column: "CreatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_CreatedByUserId",
                table: "Releases",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_DeletedAt",
                table: "Releases",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_LastUpdatedByPageId",
                table: "Releases",
                column: "LastUpdatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_LastUpdatedByUserId",
                table: "Releases",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_MasterReleaseId",
                table: "Releases",
                column: "MasterReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_OwnedByPageId",
                table: "Releases",
                column: "OwnedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_PublicId",
                table: "Releases",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseStyles_StyleId",
                table: "ReleaseStyles",
                column: "StyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Styles_Name",
                table: "Styles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AssigneeId",
                table: "SupportTickets",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CreatedByPageId",
                table: "SupportTickets",
                column: "CreatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_CreatedByUserId",
                table: "SupportTickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_LastUpdatedByPageId",
                table: "SupportTickets",
                column: "LastUpdatedByPageId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_LastUpdatedByUserId",
                table: "SupportTickets",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_Status",
                table: "SupportTickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TargetEntityType_TargetEntityId",
                table: "SupportTickets",
                columns: new[] { "TargetEntityType", "TargetEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_TicketNumber",
                table: "SupportTickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_LastUpdatedByUserId",
                table: "SystemSettings",
                column: "LastUpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SenderId",
                table: "TicketMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SenderPageId",
                table: "TicketMessages",
                column: "SenderPageId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_SupportTicketId",
                table: "TicketMessages",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_ArtistPageId",
                table: "TrackArtists",
                column: "ArtistPageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_DeletedAt",
                table: "Tracks",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_MasterTrackId",
                table: "Tracks",
                column: "MasterTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_PublicId",
                table: "Tracks",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ReleaseId",
                table: "Tracks",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReviews_AuthorPageId",
                table: "TransactionReviews",
                column: "AuthorPageId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReviews_AuthorUserId",
                table: "TransactionReviews",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReviews_ListingId",
                table: "TransactionReviews",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReviews_OrderId",
                table: "TransactionReviews",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionReviews_TargetUserId",
                table: "TransactionReviews",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDigitalLibraries_ReleaseId",
                table: "UserDigitalLibraries",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDigitalLibraries_TrackId",
                table: "UserDigitalLibraries",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDigitalLibraries_UserId",
                table: "UserDigitalLibraries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedAt",
                table: "Users",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "ListingIdentifiers");

            migrationBuilder.DropTable(
                name: "ListingImages");

            migrationBuilder.DropTable(
                name: "MasterReleaseArtists");

            migrationBuilder.DropTable(
                name: "MasterReleaseGenres");

            migrationBuilder.DropTable(
                name: "MasterReleaseLabels");

            migrationBuilder.DropTable(
                name: "MasterReleaseStyles");

            migrationBuilder.DropTable(
                name: "MasterTrackArtists");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderMessages");

            migrationBuilder.DropTable(
                name: "PageUserRoles");

            migrationBuilder.DropTable(
                name: "RedemptionCodes");

            migrationBuilder.DropTable(
                name: "ReleaseArtists");

            migrationBuilder.DropTable(
                name: "ReleaseComments");

            migrationBuilder.DropTable(
                name: "ReleaseGenres");

            migrationBuilder.DropTable(
                name: "ReleaseIdentifiers");

            migrationBuilder.DropTable(
                name: "ReleaseImages");

            migrationBuilder.DropTable(
                name: "ReleaseLabels");

            migrationBuilder.DropTable(
                name: "ReleaseStyles");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "TicketMessages");

            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropTable(
                name: "TransactionReviews");

            migrationBuilder.DropTable(
                name: "UserDigitalLibraries");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropTable(
                name: "SupportTickets");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "MasterTracks");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "MasterReleases");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
