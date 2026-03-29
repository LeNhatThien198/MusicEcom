using Backend_API.Models;
using Backend_API.Models.Enums;
using Backend_API.Models.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Backend_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<PageUserRole> PageUserRoles { get; set; }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<MasterRelease> MasterReleases { get; set; }
        public DbSet<MasterTrack> MasterTracks { get; set; }

        public DbSet<MasterReleaseGenre> MasterReleaseGenres { get; set; }
        public DbSet<MasterReleaseStyle> MasterReleaseStyles { get; set; }
        public DbSet<MasterReleaseArtist> MasterReleaseArtists { get; set; }
        public DbSet<MasterReleaseLabel> MasterReleaseLabels { get; set; }
        public DbSet<MasterTrackArtist> MasterTrackArtists { get; set; }

        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseImage> ReleaseImages { get; set; }
        public DbSet<ReleaseIdentifier> ReleaseIdentifiers { get; set; }
        public DbSet<Track> Tracks { get; set; }

        public DbSet<ReleaseGenre> ReleaseGenres { get; set; }
        public DbSet<ReleaseStyle> ReleaseStyles { get; set; }
        public DbSet<ReleaseArtist> ReleaseArtists { get; set; }
        public DbSet<ReleaseLabel> ReleaseLabels { get; set; }
        public DbSet<TrackArtist> TrackArtists { get; set; }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<ListingIdentifier> ListingIdentifiers { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<UserDigitalLibrary> UserDigitalLibraries { get; set; }
        public DbSet<RedemptionCode> RedemptionCodes { get; set; }

        public DbSet<ReleaseComment> ReleaseComments { get; set; }
        public DbSet<TransactionReview> TransactionReviews { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        public DbSet<OrderMessage> OrderMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region IDENTITY & SYSTEM

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasQueryFilter(u => u.DeletedAt == null);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.DeletedAt);

                entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
                entity.Property(u => u.Username).HasMaxLength(50).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
                entity.Property(u => u.FullName).HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.Property(u => u.Nationality).HasMaxLength(50);
                entity.Property(u => u.AvatarUrl).HasMaxLength(2048);

                entity.Property(u => u.Bio).HasColumnType("nvarchar(max)");
                entity.Property(u => u.SellerTerms).HasColumnType("nvarchar(max)");

                entity.Property(u => u.SystemRole).HasDefaultValue(SystemRole.User);
                entity.Property(u => u.Status).HasDefaultValue(AccountStatus.Active);
                entity.Property(u => u.SellerRating).HasDefaultValue(0.0f);
                entity.Property(u => u.TotalSellerReviews).HasDefaultValue(0);
                entity.Property(u => u.BuyerRating).HasDefaultValue(0.0f);
                entity.Property(u => u.TotalBuyerReviews).HasDefaultValue(0);

                entity.Property(u => u.IsEmailVerified).HasDefaultValue(false);
                entity.Property(u => u.IsTermsAccepted).HasDefaultValue(false);

                entity.Property(u => u.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(e => e.Slug).HasMaxLength(255).IsRequired();
                entity.HasIndex(e => e.Slug).IsUnique(); 

                entity.HasQueryFilter(p => p.DeletedAt == null);
                entity.HasIndex(p => p.DeletedAt);

                entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                entity.Property(p => p.OriginCountry).HasMaxLength(50);
                entity.Property(p => p.ContactEmail).HasMaxLength(256);
                entity.Property(p => p.ContactPhone).HasMaxLength(20);
                entity.Property(p => p.WebsiteUrl).HasMaxLength(2048);
                entity.Property(p => p.AvatarUrl).HasMaxLength(2048);
                entity.Property(p => p.CoverUrl).HasMaxLength(2048);

                entity.Property(p => p.Bio).HasColumnType("nvarchar(max)");
                entity.Property(p => p.SellerTerms).HasColumnType("nvarchar(max)");

                entity.Property(p => p.Status).HasDefaultValue(AccountStatus.Active);
                entity.Property(p => p.TotalReleaseComments).HasDefaultValue(0);

                entity.Property(p => p.IsVerified).HasDefaultValue(false);

                entity.Property(p => p.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(p => p.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(p => p.CreatedByUserId).IsRequired();
                entity.HasOne(p => p.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(p => p.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LastUpdatedByUser)
                      .WithMany() 
                      .HasForeignKey(e => e.LastUpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict); 

            });

            modelBuilder.Entity<PageUserRole>(entity =>
            {
                entity.HasKey(pur => new { pur.PageId, pur.UserId });
                entity.Property(pur => pur.AssignedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(pur => pur.Page).WithMany(p => p.UserRoles).HasForeignKey(pur => pur.PageId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pur => pur.User).WithMany(u => u.PageRoles).HasForeignKey(pur => pur.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_Address_Owner",
                    "([UserId] IS NOT NULL AND [PageId] IS NULL) OR ([UserId] IS NULL AND [PageId] IS NOT NULL)"));

                entity.Property(a => a.ContactName).HasMaxLength(100);
                entity.Property(a => a.PhoneNumber).HasMaxLength(20);
                entity.Property(a => a.Country).HasMaxLength(50);
                entity.Property(a => a.Province).HasMaxLength(100);
                entity.Property(a => a.District).HasMaxLength(100);
                entity.Property(a => a.Ward).HasMaxLength(100);
                entity.Property(a => a.DetailedAddress).HasMaxLength(500);
                entity.Property(a => a.CompanyName).HasMaxLength(255);
                entity.Property(a => a.TaxCode).HasMaxLength(50);

                entity.Property(a => a.Purpose).HasDefaultValue(AddressPurpose.None);
                entity.Property(a => a.IsDefault).HasDefaultValue(false);

                entity.Property(a => a.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(a => a.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.Page).WithMany(p => p.Addresses).HasForeignKey(a => a.PageId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.EntityName).HasMaxLength(100);
                entity.Property(a => a.EntityId).HasMaxLength(100);
                entity.Property(a => a.IpAddress).HasMaxLength(45);

                entity.Property(a => a.OldData).HasColumnType("nvarchar(max)");
                entity.Property(a => a.NewData).HasColumnType("nvarchar(max)");
                entity.Property(a => a.Description).HasColumnType("nvarchar(max)");

                entity.Property(a => a.Action).HasDefaultValue(AuditAction.None);

                entity.Property(a => a.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(a => a.UserId).IsRequired();
                entity.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).HasMaxLength(100);
                entity.Property(s => s.Value).HasColumnType("nvarchar(max)").IsRequired();
                entity.Property(s => s.Description).HasColumnType("nvarchar(max)");
                entity.Property(s => s.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(s => s.LastUpdatedByUserId).IsRequired();
                entity.HasOne(s => s.LastUpdatedByUser)
                      .WithMany()
                      .HasForeignKey(s => s.LastUpdatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region PIM 

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.HasIndex(g => g.Name).IsUnique();
                entity.Property(g => g.Name).HasMaxLength(50).IsRequired();
                entity.Property(g => g.Description).HasColumnType("nvarchar(max)");
                entity.Property(g => g.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(g => g.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.Name).IsUnique();
                entity.Property(s => s.Name).HasMaxLength(50).IsRequired();
                entity.Property(s => s.Description).HasColumnType("nvarchar(max)");
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(s => s.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
            });

            modelBuilder.Entity<MasterRelease>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.HasQueryFilter(m => m.DeletedAt == null);
                entity.HasIndex(m => m.DeletedAt);

                entity.Property(e => e.PublicId).UseIdentityColumn(1, 1).IsRequired();
                entity.HasIndex(e => e.PublicId).IsUnique(); 

                entity.Property(m => m.Title).HasMaxLength(255).IsRequired();
                entity.Property(m => m.CountryOfOrigin).HasMaxLength(50);
                entity.Property(m => m.CoverImageUrl).HasMaxLength(2048);

                entity.Property(m => m.Category).HasDefaultValue(ReleaseCategory.Album);
                entity.Property(m => m.Status).HasDefaultValue(EntityStatus.Draft);
                entity.Property(m => m.TotalReleaseComments).HasDefaultValue(0);
                entity.Property(m => m.ListingRating).HasDefaultValue(0.0f);
                entity.Property(m => m.TotalListingReviews).HasDefaultValue(0);

                entity.Property(m => m.IsSensitive).HasDefaultValue(false);
                entity.Property(m => m.IsExplicit).HasDefaultValue(false);

                entity.Property(m => m.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(m => m.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(m => m.CreatedByUserId).IsRequired();
                entity.HasOne(m => m.CreatedByUser).WithMany().HasForeignKey(m => m.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(m => m.CreatedByPage).WithMany().HasForeignKey(m => m.CreatedByPageId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByPage).WithMany().HasForeignKey(e => e.LastUpdatedByPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MasterTrack>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title).HasMaxLength(255).IsRequired();
                entity.Property(t => t.Position).HasMaxLength(10);
                entity.Property(t => t.IsExplicit).HasDefaultValue(false);

                entity.Property(t => t.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(t => t.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(t => t.MasterRelease).WithMany(m => m.MasterTracks).HasForeignKey(t => t.MasterReleaseId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MasterReleaseGenre>().HasKey(mrg => new { mrg.MasterReleaseId, mrg.GenreId });
            modelBuilder.Entity<MasterReleaseGenre>().HasOne(m => m.MasterRelease).WithMany(m => m.Genres).HasForeignKey(m => m.MasterReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MasterReleaseGenre>().HasOne(m => m.Genre).WithMany(g => g.MasterReleases).HasForeignKey(m => m.GenreId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MasterReleaseStyle>().HasKey(mrs => new { mrs.MasterReleaseId, mrs.StyleId });
            modelBuilder.Entity<MasterReleaseStyle>().HasOne(m => m.MasterRelease).WithMany(m => m.Styles).HasForeignKey(m => m.MasterReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MasterReleaseStyle>().HasOne(m => m.Style).WithMany(s => s.MasterReleases).HasForeignKey(m => m.StyleId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MasterReleaseArtist>(entity =>
            {
                entity.HasKey(mra => new { mra.MasterReleaseId, mra.ArtistPageId });
                entity.Property(mra => mra.IsHidden).HasDefaultValue(false);
                entity.HasOne(m => m.MasterRelease).WithMany(m => m.Artists).HasForeignKey(m => m.MasterReleaseId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.ArtistPage).WithMany().HasForeignKey(m => m.ArtistPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MasterReleaseLabel>(entity =>
            {
                entity.HasKey(mrl => new { mrl.MasterReleaseId, mrl.LabelPageId });
                entity.Property(mrl => mrl.IsHidden).HasDefaultValue(true);
                entity.HasOne(m => m.MasterRelease).WithMany(m => m.Labels).HasForeignKey(m => m.MasterReleaseId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.LabelPage).WithMany().HasForeignKey(m => m.LabelPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MasterTrackArtist>(entity =>
            {
                entity.HasKey(mta => new { mta.MasterTrackId, mta.ArtistPageId });
                entity.Property(mta => mta.Role).HasMaxLength(50).HasDefaultValue("Featuring");
                entity.HasOne(m => m.MasterTrack).WithMany(t => t.Artists).HasForeignKey(m => m.MasterTrackId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(m => m.ArtistPage).WithMany().HasForeignKey(m => m.ArtistPageId).OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region COMMERCIAL RELEASE

            modelBuilder.Entity<Release>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(e => e.PublicId).UseIdentityColumn(1, 1).IsRequired();
                entity.HasIndex(e => e.PublicId).IsUnique();

                entity.HasQueryFilter(r => r.DeletedAt == null);
                entity.HasIndex(r => r.DeletedAt);

                entity.Property(r => r.Title).HasMaxLength(255).IsRequired();
                entity.Property(r => r.Country).HasMaxLength(50);
                entity.Property(r => r.Notes).HasColumnType("nvarchar(max)");

                entity.Property(r => r.CostPrice).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Price).HasColumnType("decimal(18,2)");
                entity.Property(r => r.DiscountPrice).HasColumnType("decimal(18,2)");
                entity.Property(r => r.DigitalPrice).HasColumnType("decimal(18,2)");
                entity.Property(r => r.DigitalDiscountPrice).HasColumnType("decimal(18,2)");

                entity.Property(r => r.Edition).HasDefaultValue(ReleaseEdition.None);
                entity.Property(r => r.Format).HasDefaultValue(MediaFormat.None);
                entity.Property(r => r.StockQuantity).HasDefaultValue(0);
                entity.Property(r => r.IsPreOrder).HasDefaultValue(false);
                entity.Property(r => r.IsSensitive).HasDefaultValue(false);
                entity.Property(r => r.IsExplicit).HasDefaultValue(false);
                entity.Property(r => r.Status).HasDefaultValue(EntityStatus.Draft);

                entity.Property(r => r.TotalComments).HasDefaultValue(0);
                entity.Property(r => r.ListingRating).HasDefaultValue(0.0f);
                entity.Property(r => r.TotalListingReviews).HasDefaultValue(0);

                entity.Property(r => r.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(r => r.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(r => r.CreatedByUserId).IsRequired();
                entity.HasOne(r => r.MasterRelease).WithMany(m => m.Releases).HasForeignKey(r => r.MasterReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.CreatedByUser).WithMany().HasForeignKey(r => r.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.CreatedByPage).WithMany().HasForeignKey(r => r.CreatedByPageId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.OwnedByPage).WithMany(p => p.OwnedReleases).HasForeignKey(r => r.OwnedByPageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByPage).WithMany().HasForeignKey(e => e.LastUpdatedByPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(e => e.PublicId).UseIdentityColumn(1, 1).IsRequired();
                entity.HasIndex(e => e.PublicId).IsUnique();

                entity.HasQueryFilter(t => t.DeletedAt == null);
                entity.HasIndex(t => t.DeletedAt);

                entity.Property(t => t.Title).HasMaxLength(255).IsRequired();
                entity.Property(t => t.Position).HasMaxLength(10);
                entity.Property(t => t.PreviewMp3Url).HasMaxLength(2048);
                entity.Property(t => t.OriginalFlacUrl).HasMaxLength(2048);

                entity.Property(t => t.Price).HasColumnType("decimal(18,2)");
                entity.Property(t => t.IsExplicit).HasDefaultValue(false);

                entity.Property(t => t.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(t => t.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(t => t.Release).WithMany(r => r.Tracks).HasForeignKey(t => t.ReleaseId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(t => t.MasterTrack).WithMany(m => m.Tracks).HasForeignKey(t => t.MasterTrackId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ReleaseImage>(entity =>
            {
                entity.HasKey(ri => ri.Id);
                entity.Property(ri => ri.ImageUrl).HasMaxLength(2048).IsRequired();
                entity.Property(ri => ri.Category).HasDefaultValue(ImageCategory.None);
                entity.Property(ri => ri.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(ri => ri.SortOrder).HasDefaultValue(0);
                entity.HasIndex(ri => new { ri.ReleaseId, ri.SortOrder });
                entity.HasOne(ri => ri.Release).WithMany(r => r.Images).HasForeignKey(ri => ri.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReleaseIdentifier>(entity =>
            {
                entity.HasKey(ri => ri.Id);
                entity.Property(ri => ri.Description).HasMaxLength(255);
                entity.Property(ri => ri.Value).HasMaxLength(255).IsRequired();
                entity.Property(ri => ri.Category).HasDefaultValue(IdentifierCategory.None);
                entity.Property(ri => ri.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.HasOne(ri => ri.Release).WithMany(r => r.Identifiers).HasForeignKey(ri => ri.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReleaseGenre>().HasKey(rg => new { rg.ReleaseId, rg.GenreId });
            modelBuilder.Entity<ReleaseGenre>().HasOne(rg => rg.Release).WithMany(r => r.Genres).HasForeignKey(rg => rg.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReleaseGenre>().HasOne(rg => rg.Genre).WithMany(g => g.Releases).HasForeignKey(rg => rg.GenreId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReleaseStyle>().HasKey(rs => new { rs.ReleaseId, rs.StyleId });
            modelBuilder.Entity<ReleaseStyle>().HasOne(rs => rs.Release).WithMany(r => r.Styles).HasForeignKey(rs => rs.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReleaseStyle>().HasOne(rs => rs.Style).WithMany(s => s.Releases).HasForeignKey(rs => rs.StyleId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReleaseArtist>().HasKey(ra => new { ra.ReleaseId, ra.ArtistPageId });
            modelBuilder.Entity<ReleaseArtist>().Property(ra => ra.Role).HasDefaultValue(CollaborationRole.CreditOnly);
            modelBuilder.Entity<ReleaseArtist>().HasOne(ra => ra.Release).WithMany(r => r.Artists).HasForeignKey(ra => ra.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReleaseArtist>().HasOne(ra => ra.ArtistPage).WithMany().HasForeignKey(ra => ra.ArtistPageId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReleaseLabel>().HasKey(rl => new { rl.ReleaseId, rl.LabelPageId });
            modelBuilder.Entity<ReleaseLabel>().Property(rl => rl.Role).HasDefaultValue(CollaborationRole.CreditOnly);
            modelBuilder.Entity<ReleaseLabel>().HasOne(rl => rl.Release).WithMany(r => r.Labels).HasForeignKey(rl => rl.ReleaseId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReleaseLabel>().HasOne(rl => rl.LabelPage).WithMany().HasForeignKey(rl => rl.LabelPageId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrackArtist>().HasKey(ta => new { ta.TrackId, ta.ArtistPageId });
            modelBuilder.Entity<TrackArtist>().Property(ta => ta.Role).HasMaxLength(50).HasDefaultValue("Featuring");
            modelBuilder.Entity<TrackArtist>().HasOne(ta => ta.Track).WithMany(t => t.Artists).HasForeignKey(ta => ta.TrackId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TrackArtist>().HasOne(ta => ta.ArtistPage).WithMany().HasForeignKey(ta => ta.ArtistPageId).OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region COMMERCE & SECONDARY MARKET

            modelBuilder.Entity<Listing>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.Property(e => e.PublicId).UseIdentityColumn(10000, 1).IsRequired(); 
                entity.HasIndex(e => e.PublicId).IsUnique();

                entity.HasQueryFilter(l => l.DeletedAt == null);
                entity.HasIndex(l => l.DeletedAt);

                entity.Property(l => l.Title).HasMaxLength(255).IsRequired();
                entity.Property(l => l.Notes).HasColumnType("nvarchar(max)");

                entity.Property(l => l.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(l => l.CostPrice).HasColumnType("decimal(18,2)");

                entity.Property(l => l.MediaCondition).HasDefaultValue(ListingCondition.None);
                entity.Property(l => l.SleeveCondition).HasDefaultValue(ListingCondition.None);
                entity.Property(l => l.StockQuantity).HasDefaultValue(1);
                entity.Property(l => l.IsSensitive).HasDefaultValue(false);
                entity.Property(l => l.Status).HasDefaultValue(EntityStatus.Draft);

                entity.Property(l => l.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(l => l.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(l => l.SellerId).IsRequired();
                entity.HasOne(l => l.Release).WithMany(r => r.Listings).HasForeignKey(l => l.ReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(l => l.Seller).WithMany(u => u.Listings).HasForeignKey(l => l.SellerId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ListingImage>(entity =>
            {
                entity.HasKey(li => li.Id);
                entity.Property(li => li.ImageUrl).HasMaxLength(2048).IsRequired();
                entity.Property(li => li.Category).HasDefaultValue(ImageCategory.None);
                entity.Property(li => li.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.HasOne(li => li.Listing).WithMany(l => l.Images).HasForeignKey(li => li.ListingId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ListingIdentifier>(entity =>
            {
                entity.HasKey(li => li.Id);
                entity.Property(li => li.Description).HasMaxLength(255);
                entity.Property(li => li.Value).HasMaxLength(255).IsRequired();
                entity.Property(li => li.Category).HasDefaultValue(IdentifierCategory.None);
                entity.Property(li => li.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.HasOne(li => li.Listing).WithMany(l => l.Identifiers).HasForeignKey(li => li.ListingId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.UserId);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(c => c.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.HasOne(c => c.User).WithOne(u => u.Cart).HasForeignKey<Cart>(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_CartItem_ItemType",
                    "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL AND [ListingId] IS NULL) OR " +
                    "([ReleaseId] IS NULL AND [TrackId] IS NOT NULL AND [ListingId] IS NULL) OR " +
                    "([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NOT NULL) OR " +
                    "([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NULL)"));

                entity.Property(ci => ci.Category).HasDefaultValue(OrderItemCategory.None);
                entity.Property(ci => ci.Quantity).HasDefaultValue(1);
                entity.Property(ci => ci.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(ci => ci.Cart).WithMany(c => c.CartItems).HasForeignKey(ci => ci.CartId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Release).WithMany().HasForeignKey(ci => ci.ReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ci => ci.Track).WithMany().HasForeignKey(ci => ci.TrackId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ci => ci.Listing).WithMany().HasForeignKey(ci => ci.ListingId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(e => e.OrderCode).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.OrderCode).IsUnique(); 

                entity.Property(e => e.PaymentSessionId).HasMaxLength(255); 

                entity.ToTable(tb => tb.HasCheckConstraint("CK_Order_SellerType",
                    "([SellerUserId] IS NOT NULL AND [SellerPageId] IS NULL) OR ([SellerUserId] IS NULL AND [SellerPageId] IS NOT NULL)"));

                entity.Property(o => o.ShippingContactName).HasMaxLength(100);
                entity.Property(o => o.ShippingPhoneNumber).HasMaxLength(20);
                entity.Property(o => o.ShippingFullAddress).HasMaxLength(500);
                entity.Property(o => o.BillingCompanyName).HasMaxLength(255);
                entity.Property(o => o.BillingTaxCode).HasMaxLength(50);
                entity.Property(o => o.BillingFullAddress).HasMaxLength(500);
                entity.Property(o => o.TrackingCode).HasMaxLength(100);

                entity.Property(o => o.ShippingFee).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();

                entity.Property(o => o.PaymentMethod).HasDefaultValue(PaymentMethod.None);

                entity.Property(o => o.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(o => o.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(o => o.BuyerUserId).IsRequired();

                entity.HasOne(o => o.BuyerUser).WithMany(u => u.Orders).HasForeignKey(o => o.BuyerUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.SellerUser).WithMany().HasForeignKey(o => o.SellerUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(o => o.SellerPage).WithMany().HasForeignKey(o => o.SellerPageId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByPage).WithMany().HasForeignKey(e => e.LastUpdatedByPageId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_OrderItem_ItemType",
                    "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL AND [ListingId] IS NULL) OR " +
                    "([ReleaseId] IS NULL AND [TrackId] IS NOT NULL AND [ListingId] IS NULL) OR " +
                    "([ReleaseId] IS NULL AND [TrackId] IS NULL AND [ListingId] IS NOT NULL)"));

                entity.Property(oi => oi.Category).HasDefaultValue(OrderItemCategory.None);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(oi => oi.SubTotal).HasColumnType("decimal(18,2)").IsRequired();

                entity.HasOne(oi => oi.Order).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Release).WithMany().HasForeignKey(oi => oi.ReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oi => oi.Track).WithMany().HasForeignKey(oi => oi.TrackId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(oi => oi.Listing).WithMany().HasForeignKey(oi => oi.ListingId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserDigitalLibrary>(entity =>
            {
                entity.HasKey(udl => udl.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_UserDigitalLibrary_ItemType",
                    "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL)"));

                entity.Property(udl => udl.Method).HasDefaultValue(AcquisitionMethod.None);
                entity.Property(udl => udl.IsUnlocked).HasDefaultValue(true);
                entity.Property(udl => udl.AcquiredAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(udl => udl.UserId).IsRequired();

                entity.HasOne(udl => udl.User).WithMany(u => u.DigitalLibraries).HasForeignKey(udl => udl.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(udl => udl.Release).WithMany().HasForeignKey(udl => udl.ReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(udl => udl.Track).WithMany().HasForeignKey(udl => udl.TrackId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RedemptionCode>(entity =>
            {
                entity.HasKey(rc => rc.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_RedemptionCode_ItemType",
                    "([ReleaseId] IS NOT NULL AND [TrackId] IS NULL) OR ([ReleaseId] IS NULL AND [TrackId] IS NOT NULL)"));

                entity.HasIndex(rc => rc.Code).IsUnique();
                entity.Property(rc => rc.Code).HasMaxLength(50).IsRequired();
                entity.Property(rc => rc.MaxUses).HasDefaultValue(1);
                entity.Property(rc => rc.CurrentUses).HasDefaultValue(0);
                entity.Property(rc => rc.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(rc => rc.CreatedByPageId).IsRequired();

                entity.HasOne(rc => rc.Release).WithMany().HasForeignKey(rc => rc.ReleaseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(rc => rc.Track).WithMany().HasForeignKey(rc => rc.TrackId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(rc => rc.CreatedByPage).WithMany(p => p.RedemptionCodes).HasForeignKey(rc => rc.CreatedByPageId).OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region ENGAGEMENT, COMMUNICATION & SUPPORT

            modelBuilder.Entity<ReleaseComment>(entity =>
            {
                entity.HasKey(rc => rc.Id);

                entity.Property(rc => rc.Content).HasMaxLength(2000).IsRequired();

                entity.Property(rc => rc.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(rc => rc.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(rc => rc.Release).WithMany(r => r.Comments).HasForeignKey(rc => rc.ReleaseId).OnDelete(DeleteBehavior.Cascade);

                entity.Property(rc => rc.UserId).IsRequired();
                entity.HasOne(rc => rc.User).WithMany(u => u.Comments).HasForeignKey(rc => rc.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(rc => rc.Page).WithMany(p => p.Comments).HasForeignKey(rc => rc.PageId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rc => rc.ParentComment).WithMany(p => p.Replies).HasForeignKey(rc => rc.ParentCommentId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<TransactionReview>(entity =>
            {
                entity.HasKey(tr => tr.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_TransactionReview_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

                entity.Property(tr => tr.Role).HasDefaultValue(ReviewRole.None);
                entity.Property(tr => tr.Content).HasMaxLength(1000);
                entity.Property(tr => tr.ReplyContent).HasMaxLength(1000);

                entity.Property(tr => tr.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(tr => tr.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(tr => tr.AuthorUserId).IsRequired();
                entity.Property(tr => tr.TargetUserId).IsRequired();

                entity.HasOne(tr => tr.Order).WithMany(o => o.Reviews).HasForeignKey(tr => tr.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(tr => tr.Listing).WithMany(l => l.Reviews).HasForeignKey(tr => tr.ListingId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(tr => tr.AuthorUser).WithMany(u => u.WrittenReviews).HasForeignKey(tr => tr.AuthorUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(tr => tr.AuthorPage).WithMany().HasForeignKey(tr => tr.AuthorPageId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(tr => tr.TargetUser).WithMany(u => u.ReceivedReviews).HasForeignKey(tr => tr.TargetUserId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.HasKey(st => st.Id);
                entity.Property(e => e.TicketNumber).UseIdentityColumn(1000, 1).IsRequired();
                entity.HasIndex(e => e.TicketNumber).IsUnique();

                entity.HasIndex(st => st.Status);
                entity.HasIndex(st => new { st.TargetEntityType, st.TargetEntityId });

                entity.Property(st => st.Title).HasMaxLength(255).IsRequired();
                entity.Property(st => st.Description).HasMaxLength(4000).IsRequired();

                entity.Property(st => st.TargetEntityType).HasMaxLength(100);

                entity.Property(st => st.Category).HasDefaultValue(TicketCategory.None);

                entity.Property(st => st.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");
                entity.Property(st => st.UpdatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(st => st.CreatedByUserId).IsRequired();

                entity.HasOne(st => st.CreatedByUser).WithMany(u => u.SupportTickets).HasForeignKey(st => st.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(st => st.CreatedByPage).WithMany(p => p.SupportTickets).HasForeignKey(st => st.CreatedByPageId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByUser).WithMany().HasForeignKey(e => e.LastUpdatedByUserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.LastUpdatedByPage).WithMany().HasForeignKey(e => e.LastUpdatedByPageId).OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.Assignee).WithMany().HasForeignKey(st => st.AssigneeId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TicketMessage>(entity =>
            {
                entity.HasKey(tm => tm.Id);

                entity.Property(tm => tm.MessageText).HasMaxLength(4000).IsRequired();
                entity.Property(tm => tm.AttachmentUrl).HasMaxLength(2048);
                entity.Property(tm => tm.IsRead).HasDefaultValue(false);

                entity.Property(tm => tm.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(tm => tm.SenderId).IsRequired();

                entity.HasOne(tm => tm.SupportTicket).WithMany(st => st.Messages).HasForeignKey(tm => tm.SupportTicketId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(tm => tm.Sender).WithMany().HasForeignKey(tm => tm.SenderId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(tm => tm.SenderPage).WithMany().HasForeignKey(tm => tm.SenderPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderMessage>(entity =>
            {
                entity.HasKey(om => om.Id);

                entity.Property(om => om.MessageText).HasMaxLength(2000).IsRequired();
                entity.Property(om => om.AttachmentUrl).HasMaxLength(2048);
                entity.Property(om => om.IsRead).HasDefaultValue(false);

                entity.Property(om => om.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.Property(om => om.SenderId).IsRequired();

                entity.HasOne(om => om.Order).WithMany(o => o.Messages).HasForeignKey(om => om.OrderId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(om => om.Sender).WithMany().HasForeignKey(om => om.SenderId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(om => om.SenderPage).WithMany().HasForeignKey(om => om.SenderPageId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.ToTable(tb => tb.HasCheckConstraint("CK_Notification_UserOrPage",
                    "([UserId] IS NOT NULL AND [PageId] IS NULL) OR ([UserId] IS NULL AND [PageId] IS NOT NULL)"));

                entity.HasIndex(n => n.UserId).HasFilter("[IsRead] = 0");
                entity.HasIndex(n => n.PageId).HasFilter("[IsRead] = 0");

                entity.Property(n => n.Title).HasMaxLength(255).IsRequired();
                entity.Property(n => n.Message).HasMaxLength(2000).IsRequired();
                entity.Property(n => n.ActionUrl).HasMaxLength(2048);

                entity.Property(n => n.Category).HasDefaultValue(NotificationCategory.None);
                entity.Property(n => n.IsRead).HasDefaultValue(false);

                entity.Property(n => n.CreatedAt).HasDefaultValueSql("SYSDATETIMEOFFSET()");

                entity.HasOne(n => n.User).WithMany(u => u.Notifications).HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(n => n.Page).WithMany(p => p.Notifications).HasForeignKey(n => n.PageId).OnDelete(DeleteBehavior.Cascade);
            });

            #endregion
        }
    }
}
