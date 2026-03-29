using Backend_API.Data;
using Backend_API.Helpers;
using Backend_API.Models;
using Backend_API.Models.Enums;
using Backend_API.Models.Relations;
using Backend_API.Services.Media;
using DiscogsCrawler.Helpers;
using DiscogsCrawler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DiscogsCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            string CONNECTION_STRING = config.GetConnectionString("DefaultConnection") ?? "";
            string CLOUDINARY_URL = config["Cloudinary:Url"] ?? "";
            string DISCOGS_TOKEN = config["Discogs:Token"] ?? "";

            while (true)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("    SIÊU CỖ MÁY ETL & SEED DATA (DISCOGS -> CLOUDINARY -> SQL)  ");
                Console.WriteLine("==========================================================");
                Console.WriteLine("1. Chạy Máy cào dữ liệu (Scraper V4 - Đã tối ưu)");
                Console.WriteLine("2. [PHASE 1] Máy nội soi - Sinh Bản vẽ Dữ liệu (Blueprint) ra file JSON");
                Console.WriteLine("3. [PHASE 2 - DRY RUN] Rà soát Bản vẽ & Kiểm tra File vật lý");
                Console.WriteLine("4. [PHASE 3 - EXECUTE] Kích hoạt Động cơ Bơm - Đọc Bản vẽ -> Đẩy Mây -> SQL");
                Console.WriteLine("0. Thoát");
                Console.WriteLine("==========================================================");
                Console.Write("Mời Sếp ra lệnh (0-4): ");

                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    await ScraperEngine.RunAsync(DISCOGS_TOKEN);
                    Console.WriteLine("\n[Hoàn tất thao tác cào] Nhấn phím bất kỳ..."); Console.ReadKey();
                }
                else if (choice == "2")
                {
                    await SeederEngine.GenerateBlueprintAsync();
                    Console.WriteLine("\n[Hoàn tất Phase 1] Kiểm tra JSON tại ổ D! Nhấn phím bất kỳ..."); Console.ReadKey();
                }
                else if (choice == "3")
                {
                    await SeederEngine.ValidateBlueprintAsync();
                    Console.WriteLine("\n[Hoàn tất Rà soát 2] Nhấn phím bất kỳ để về Menu chính..."); Console.ReadKey();
                }
                else if (choice == "4")
                {
                    await SeederEngine.ExecuteBlueprintAsync(CONNECTION_STRING, CLOUDINARY_URL);
                    Console.WriteLine("\n[Hoàn tất Phase 3] Bơm Cloudinary và DB thành công! Nhấn phím bất kỳ..."); Console.ReadKey();
                }
                else if (choice == "0") break;
            }
        }
    }

    public static class SeederEngine
    {
        private const string BLUEPRINT_PATH = @"D:\DoAnThucTapCuoiKhoa\DataLake\Seeding_Blueprint.json";

        public static async Task GenerateBlueprintAsync()
        {
            Console.Clear();
            Console.WriteLine("=== [PHASE 1] LÊN BẢN VẼ CHI TIẾT (CHUẨN CLOUDINARY & METADATA) ===");
            var blueprint = new SeedingBlueprint();

            string artistsPath = @"D:\DoAnThucTapCuoiKhoa\DataLake\Artists_And_Bands";
            string labelsPath = @"D:\DoAnThucTapCuoiKhoa\DataLake\Labels";

            var admin = new BpUser { Id = Guid.NewGuid(), Username = "admin_master", Email = "admin@musicecom.com", FullName = "System Admin", SystemRole = SystemRole.Admin };
            var thien = new BpUser { Id = Guid.NewGuid(), Username = "thien", Email = "Lenhatthien198@gmail.com", FullName = "Thien", SystemRole = SystemRole.Admin };

            var staff1 = new BpUser { Id = Guid.NewGuid(), Username = "staff_01", Email = "staff1@musicecom.com", FullName = "Content Moderator", SystemRole = SystemRole.Staff };
            var staff2 = new BpUser { Id = Guid.NewGuid(), Username = "staff_02", Email = "staff2@musicecom.com", FullName = "Support Agent", SystemRole = SystemRole.Staff };

            var freeUser1 = new BpUser { Id = Guid.NewGuid(), Username = "buyer_john", Email = "john.doe@gmail.com", FullName = "John Doe (Buyer)", SystemRole = SystemRole.User };
            var freeUser2 = new BpUser { Id = Guid.NewGuid(), Username = "collector_jane", Email = "jane.smith@gmail.com", FullName = "Jane Smith (Collector)", SystemRole = SystemRole.User };
            var freeUser3 = new BpUser { Id = Guid.NewGuid(), Username = "seller_mike", Email = "mike.seller@yahoo.com", FullName = "Mike (Used Seller)", SystemRole = SystemRole.User };

            blueprint.Users.AddRange(new[] { admin, thien, staff1, staff2, freeUser1, freeUser2, freeUser3 });
            var labelCache = new Dictionary<string, BpPage>();
            int userIndex = 1;

            if (Directory.Exists(labelsPath))
            {
                foreach (var lblFolder in Directory.GetDirectories(labelsPath))
                {
                    string infoFile = Directory.GetFiles(lblFolder, "*.json").FirstOrDefault() ?? "";
                    if (string.IsNullOrEmpty(infoFile)) continue;

                    string rawLabelJson = await System.IO.File.ReadAllTextAsync(infoFile);
                    var lblJson = JObject.Parse(DiscogsCleaner.CleanRawJson(rawLabelJson));
                    string lblName = lblJson["name"]?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(lblName)) continue;

                    string cleanName = lblName.ToLower().Replace("_records_label", "").Replace(" records", "").Trim();
                    string slug = StringHelper.GenerateSlug(lblName);

                    var owner = new BpUser { Id = Guid.NewGuid(), Username = $"owner_{slug}", Email = $"owner_{slug}@gmail.com", SystemRole = SystemRole.User };
                    var manager = new BpUser { Id = Guid.NewGuid(), Username = $"manager_{slug}", Email = $"manager_{slug}@gmail.com", SystemRole = SystemRole.User };
                    blueprint.Users.AddRange(new[] { owner, manager });

                    string avatarFile = Directory.GetFiles(lblFolder, "*.jpg").FirstOrDefault() ?? "";
                    var labelPage = new BpPage
                    {
                        Id = Guid.NewGuid(),
                        Name = lblName,
                        Slug = slug,
                        Category = PageCategory.Label,
                        Bio = lblJson["profile"]?.ToString() ?? "Chưa có thông tin",
                        WebsiteUrl = lblJson["urls"]?[0]?.ToString() ?? "",
                        OwnerUserId = owner.Id,
                        ManagerUserId = manager.Id,
                        CreatedByUserId = thien.Id,
                        LastUpdatedByUserId = thien.Id
                    };

                    if (!string.IsNullOrEmpty(avatarFile))
                    {
                        labelPage._AvatarUploadPlan = new MediaUploadPlan
                        {
                            LocalFilePath = avatarFile,
                            TargetCloudinaryFolder = $"MusicEcom_SeedData/Labels/{slug}/Avatars",
                            ExpectedPublicId = $"avatar-{labelPage.Id.ToString().Substring(0, 8)}"
                        };
                    }
                    blueprint.Pages.Add(labelPage);
                    labelCache[cleanName] = labelPage;
                }
            }

            if (!Directory.Exists(artistsPath)) return;
            foreach (var artistFolder in Directory.GetDirectories(artistsPath))
            {
                string infoFile = Directory.GetFiles(artistFolder, "*.json").FirstOrDefault() ?? "";
                if (string.IsNullOrEmpty(infoFile)) continue;

                string rawArtistJson = await System.IO.File.ReadAllTextAsync(infoFile);
                var artistJson = JObject.Parse(DiscogsCleaner.CleanRawJson(rawArtistJson));
                string artistName = artistJson["name"]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(artistName)) continue;
                string artistSlug = StringHelper.GenerateSlug(artistName);

                var owner = new BpUser { Id = Guid.NewGuid(), Username = $"owner_art_{userIndex}", Email = $"owner{userIndex}@gmail.com", SystemRole = SystemRole.User };
                var manager = new BpUser { Id = Guid.NewGuid(), Username = $"mgr_art_{userIndex}", Email = $"mgr{userIndex}@gmail.com", SystemRole = SystemRole.User };
                blueprint.Users.AddRange(new[] { owner, manager });

                string avatarFile = Directory.GetFiles(artistFolder, "*.jpg").FirstOrDefault() ?? "";
                var artistPage = new BpPage
                {
                    Id = Guid.NewGuid(),
                    Name = artistName,
                    Slug = artistSlug,
                    Category = PageCategory.Artist,
                    Bio = artistJson["profile"]?.ToString() ?? "Chưa có thông tin",
                    WebsiteUrl = artistJson["urls"]?[0]?.ToString() ?? "",
                    OwnerUserId = owner.Id,
                    ManagerUserId = manager.Id,
                    CreatedByUserId = thien.Id,
                    LastUpdatedByUserId = thien.Id
                };

                if (!string.IsNullOrEmpty(avatarFile))
                {
                    artistPage._AvatarUploadPlan = new MediaUploadPlan
                    {
                        LocalFilePath = avatarFile,
                        TargetCloudinaryFolder = $"MusicEcom_SeedData/Artists/{artistSlug}/Avatars",
                        ExpectedPublicId = $"avatar-{artistPage.Id.ToString().Substring(0, 8)}"
                    };
                }
                blueprint.Pages.Add(artistPage);

                string albumsPath = Path.Combine(artistFolder, "Albums");
                if (!Directory.Exists(albumsPath)) { userIndex++; continue; }

                foreach (var masterFolder in Directory.GetDirectories(albumsPath))
                {
                    string masterFile = Directory.GetFiles(masterFolder, "master_*.json").FirstOrDefault() ?? "";
                    if (string.IsNullOrEmpty(masterFile)) continue;

                    string rawMasterJson = await System.IO.File.ReadAllTextAsync(masterFile);
                    var masterJson = JObject.Parse(DiscogsCleaner.CleanRawJson(rawMasterJson));
                    string masterTitle = masterJson["title"]?.ToString() ?? "";
                    string masterSlug = StringHelper.GenerateSlug(masterTitle);
                    if (string.IsNullOrWhiteSpace(masterTitle)) continue;

                    int.TryParse(masterJson["year"]?.ToString(), out int masterYear);

                    var masterRelease = new BpMasterRelease
                    {
                        Id = Guid.NewGuid(),
                        ArtistPageId = artistPage.Id,
                        Title = masterTitle,
                        ReleaseYear = masterYear,
                        CreatedByUserId = owner.Id,
                        CreatedByPageId = artistPage.Id,
                        LastUpdatedByUserId = owner.Id,
                        LastUpdatedByPageId = artistPage.Id
                    };

                    string masterCover = Directory.GetFiles(masterFolder, "cover_master_*.jpg").FirstOrDefault() ?? "";
                    if (!string.IsNullOrEmpty(masterCover))
                    {
                        masterRelease._CoverUploadPlan = new MediaUploadPlan
                        {
                            LocalFilePath = masterCover,
                            TargetCloudinaryFolder = $"MusicEcom_SeedData/Artists/{artistSlug}/{masterSlug}/Covers",
                            ExpectedPublicId = $"master-cover-{masterRelease.Id.ToString().Substring(0, 8)}"
                        };
                    }
                    blueprint.MasterReleases.Add(masterRelease);

                    string releasesPath = Path.Combine(masterFolder, "Releases");
                    if (!Directory.Exists(releasesPath)) continue;

                    foreach (var releaseFile in Directory.GetFiles(releasesPath, "release_*.json"))
                    {
                        string releaseFileName = Path.GetFileNameWithoutExtension(releaseFile);
                        string rawReleaseJson = await System.IO.File.ReadAllTextAsync(releaseFile);
                        var relJson = JObject.Parse(DiscogsCleaner.CleanRawJson(rawReleaseJson));

                        string formatStr = relJson["formats"]?[0]?["name"]?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(formatStr)) continue;

                        string descriptions = relJson["formats"]?[0]?["descriptions"]?.ToString().ToLower() ?? "";
                        ReleaseEdition edition = ReleaseEdition.Original;
                        if (descriptions.Contains("repress")) edition = ReleaseEdition.Repress;
                        else if (descriptions.Contains("reissue")) edition = ReleaseEdition.Reissue;
                        string editionSlug = edition.ToString().ToLower();

                        string rawLabelName = relJson["labels"]?[0]?["name"]?.ToString() ?? "Independent";
                        string cleanLabelName = rawLabelName.ToLower().Replace("_records_label", "").Replace(" records", "").Trim();
                        Guid ownedByPageId = labelCache.ContainsKey(cleanLabelName) ? labelCache[cleanLabelName].Id : artistPage.Id;
                        Guid actionUserId = labelCache.ContainsKey(cleanLabelName) ? labelCache[cleanLabelName].OwnerUserId : owner.Id;

                        int.TryParse(relJson["year"]?.ToString(), out int relYear);

                        var release = new BpRelease
                        {
                            Id = Guid.NewGuid(),
                            MasterReleaseId = masterRelease.Id,
                            OwnedByPageId = ownedByPageId,
                            ArtistPageId = artistPage.Id,
                            Title = masterTitle,
                            Edition = edition,
                            Format = formatStr.Contains("CD") ? MediaFormat.CD : MediaFormat.Vinyl,
                            ReleaseYear = relYear,
                            Country = relJson["country"]?.ToString() ?? "Unknown",
                            Notes = relJson["notes"]?.ToString() ?? "",
                            Price = edition == ReleaseEdition.Original ? 800000 : 400000,
                            DigitalPrice = 0,
                            CreatedByUserId = actionUserId,
                            CreatedByPageId = ownedByPageId,
                            LastUpdatedByUserId = actionUserId,
                            LastUpdatedByPageId = ownedByPageId
                        };
                        blueprint.Releases.Add(release);

                        if (relJson["labels"] is JArray labelArray)
                        {
                            foreach (var lbl in labelArray)
                            {
                                string catNo = lbl["catno"]?.ToString() ?? "";
                                string lblName = lbl["name"]?.ToString() ?? "Unknown Label";

                                if (!string.IsNullOrWhiteSpace(catNo) && catNo.ToLower() != "none")
                                {
                                    blueprint.ReleaseIdentifiers.Add(new BpReleaseIdentifier
                                    {
                                        Id = Guid.NewGuid(),
                                        ReleaseId = release.Id,
                                        Category = IdentifierCategory.CatalogNumber,
                                        Value = catNo,
                                        Description = $"Cat# by {lblName}"
                                    });
                                }
                            }
                        }

                        if (relJson["identifiers"] is JArray identifiers)
                        {
                            foreach (var iden in identifiers)
                            {
                                string idType = iden["type"]?.ToString().ToLower() ?? "";

                                IdentifierCategory idCat = IdentifierCategory.Other;

                                if (idType.Contains("barcode")) idCat = IdentifierCategory.Barcode;
                                else if (idType.Contains("matrix") || idType.Contains("runout")) idCat = IdentifierCategory.MatrixRunout;

                                blueprint.ReleaseIdentifiers.Add(new BpReleaseIdentifier
                                {
                                    Id = Guid.NewGuid(),
                                    ReleaseId = release.Id,
                                    Category = idCat,
                                    Value = iden["value"]?.ToString() ?? "",
                                    Description = iden["description"]?.ToString() ?? idType
                                });
                            }
                        }

                        string releaseFormatSlug = formatStr.Contains("CD") ? "cd" : "vinyl";
                        string countrySlug = StringHelper.GenerateSlug(relJson["country"]?.ToString() ?? "unknown");
                        string releaseSlug = $"{releaseFormatSlug}-{editionSlug}-{relYear}-{countrySlug}";

                        string cloudImgFolder = $"MusicEcom_SeedData/Artists/{artistSlug}/{masterSlug}/Images/{releaseSlug}";

                        string primaryCoverFile = Directory.GetFiles(releasesPath, $"{releaseFileName}_cover.jpg").FirstOrDefault() ?? "";
                        if (!string.IsNullOrEmpty(primaryCoverFile))
                        {
                            blueprint.ReleaseImages.Add(new BpReleaseImage
                            {
                                Id = Guid.NewGuid(),
                                ReleaseId = release.Id,
                                Category = ImageCategory.Primary,
                                _ImageUploadPlan = new MediaUploadPlan
                                {
                                    LocalFilePath = primaryCoverFile,
                                    TargetCloudinaryFolder = cloudImgFolder,
                                    ExpectedPublicId = $"cover-primary-{Guid.NewGuid().ToString().Substring(0, 8)}"
                                }
                            });
                        }

                        var galleryFiles = Directory.GetFiles(releasesPath, $"{releaseFileName}_gallery*.jpg").Take(3);
                        foreach (var gFile in galleryFiles)
                        {
                            blueprint.ReleaseImages.Add(new BpReleaseImage
                            {
                                Id = Guid.NewGuid(),
                                ReleaseId = release.Id,
                                Category = ImageCategory.Secondary,
                                _ImageUploadPlan = new MediaUploadPlan
                                {
                                    LocalFilePath = gFile,
                                    TargetCloudinaryFolder = cloudImgFolder,
                                    ExpectedPublicId = $"gallery-{Guid.NewGuid().ToString().Substring(0, 8)}"
                                }
                            });
                        }

                        blueprint.ReleaseImages.Add(new BpReleaseImage
                        {
                            Id = Guid.NewGuid(),
                            ReleaseId = release.Id,
                            Category = ImageCategory.Matrix,
                            _ImageUploadPlan = new MediaUploadPlan
                            {
                                LocalFilePath = "",
                                TargetCloudinaryFolder = cloudImgFolder,
                                ExpectedPublicId = $"matrix-{Guid.NewGuid().ToString().Substring(0, 8)}"
                            }
                        });

                        string audioTarget = (edition == ReleaseEdition.Reissue) ? "ReissueAudio" : "OriginalAudio";
                        string audioPath = Path.Combine(masterFolder, audioTarget);

                        if (relJson["tracklist"] is JArray tracklist)
                        {
                            int trackCount = tracklist.Count;

                            var audioFiles = new List<string>();
                            if (Directory.Exists(audioPath))
                            {
                                audioFiles = Directory.GetFiles(audioPath, "*.*").Where(f => f.EndsWith(".mp3") || f.EndsWith(".m4a")).OrderBy(f => f).ToList();
                            }

                            release.DigitalPrice = audioFiles.Any() ? (trackCount * 15000) : 0;

                            string folderEdition = (edition == ReleaseEdition.Reissue) ? "Reissue" : "Original";

                            for (int i = 0; i < trackCount; i++)
                            {
                                string pos = tracklist[i]["position"]?.ToString() ?? (i + 1).ToString();
                                string trackTitle = tracklist[i]["title"]?.ToString() ?? $"Track {i + 1}";
                                string localAudio = i < audioFiles.Count ? audioFiles[i] : "";

                                int actualDuration = 0;
                                if (!string.IsNullOrEmpty(localAudio))
                                {
                                    actualDuration = MediaHelper.GetActualAudioDuration(localAudio);
                                }
                                else
                                {
                                    string jsonDuration = tracklist[i]["duration"]?.ToString() ?? "";
                                    actualDuration = DiscogsParseDuration.ParseDuration(jsonDuration);
                                }

                                var track = new BpTrack
                                {
                                    Id = Guid.NewGuid(),
                                    ReleaseId = release.Id,
                                    Title = trackTitle,
                                    Position = pos,
                                    Price = audioFiles.Any() ? 15000 : 0,
                                    DurationSeconds = actualDuration,
                                    IsExplicit = false
                                };

                                if (!string.IsNullOrEmpty(localAudio))
                                {
                                    track._AudioUploadPlan = new MediaUploadPlan
                                    {
                                        LocalFilePath = localAudio,
                                        ResourceType = "video",
                                        TargetCloudinaryFolder = $"MusicEcom_SeedData/Artists/{artistSlug}/{masterSlug}/Audio/{folderEdition}",
                                        ExpectedPublicId = $"track-{Path.GetFileNameWithoutExtension(localAudio).Replace("_E", "").Replace("_e", "")}-{Guid.NewGuid().ToString().Substring(0, 8)}"
                                    };
                                }
                                blueprint.Tracks.Add(track);
                            }
                        }
                    }
                }
                userIndex++;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            await System.IO.File.WriteAllTextAsync(BLUEPRINT_PATH, JsonSerializer.Serialize(blueprint, options));
            Console.WriteLine("[XUẤT SẮC] Đã xuất Blueprint cấu trúc phân cấp. Mời Sếp check JSON!");
        }

        public static async Task ValidateBlueprintAsync()
        {
            Console.Clear();
            Console.WriteLine("=== [DRY RUN] ĐANG RÀ SOÁT VÀ XUẤT BÁO CÁO CHI TIẾT ===");

            if (!System.IO.File.Exists(BLUEPRINT_PATH))
            {
                Console.WriteLine("[!] LỖI: Không tìm thấy Bản vẽ Blueprint. Hãy chạy Phase 1 trước!");
                return;
            }

            var blueprint = JsonSerializer.Deserialize<SeedingBlueprint>(
                await System.IO.File.ReadAllTextAsync(BLUEPRINT_PATH),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (blueprint == null) return;

            string reportPath = @"D:\DoAnThucTapCuoiKhoa\DataLake\DryRun_Report.txt";
            using StreamWriter writer = new StreamWriter(reportPath, false, System.Text.Encoding.UTF8);

            void Log(string msg, ConsoleColor color = ConsoleColor.White)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
                writer.WriteLine(msg);
            }

            Log("=========================================================================");
            Log("                      BÁO CÁO MÔ PHỎNG BƠM DỮ LIỆU                       ");
            Log("=========================================================================\n");

            Log("--- GIẢI THÍCH THUẬT NGỮ ĐỂ SẾP HIỂU RÕ ---", ConsoleColor.Yellow);
            Log("1. [File vật lý độc bản]: Là số lượng file THỰC SỰ duy nhất trên ổ cứng. (Ví dụ: Bản Original và Repress trỏ chung vào 1 file 01.mp3, hệ thống chỉ đếm là 1 file vật lý độc bản, và Cloudinary chỉ cần up 1 lần).");
            Log("2. [Cache Hit]: Khi hệ thống quét tới bản Repress và thấy file 01.mp3 đã được up lúc trước, nó sẽ 'Cache Hit' (trúng bộ nhớ đệm), tái sử dụng ngay cái URL trên RAM để chèn vào SQL mà KHÔNG TỐN MẠNG đẩy lên Cloudinary lần nữa.");
            Log("3. [PublicId - Số nguyên tự tăng]: Các bảng MasterRelease, Release, Track khi lưu vào SQL sẽ TỰ ĐỘNG được Entity Framework gán số thứ tự (1, 2, 3...). Ta không cần cấu hình trên Blueprint và không liên quan đến Cloudinary.\n");

            int missingFilesCount = 0;
            var uniqueUploads = new HashSet<string>();
            var duplicateCacheHits = new HashSet<string>();

            void SimulateCloudinary(MediaUploadPlan? plan, string sqlTable, string sqlColumn, string identifierInfo)
            {
                if (plan == null || string.IsNullOrWhiteSpace(plan.LocalFilePath)) return;

                if (!System.IO.File.Exists(plan.LocalFilePath))
                {
                    Log($"      [!] LỖI FILE VẬT LÝ: Không tìm thấy file '{plan.LocalFilePath}'", ConsoleColor.Red);
                    missingFilesCount++;
                }
                else
                {
                    if (!uniqueUploads.Contains(plan.LocalFilePath))
                    {
                        Log($"      [Cloudinary] -> UP MỚI: File '{Path.GetFileName(plan.LocalFilePath)}'");
                        Log($"                    -> Đích đến: Folder [{plan.TargetCloudinaryFolder}] | Tên file Cloud: [{plan.ExpectedPublicId}]");
                        uniqueUploads.Add(plan.LocalFilePath);
                    }
                    else
                    {
                        Log($"      [Cloudinary] -> CACHE HIT: File '{Path.GetFileName(plan.LocalFilePath)}' đã được up trước đó. Sử dụng lại URL!", ConsoleColor.Cyan);
                        duplicateCacheHits.Add(plan.LocalFilePath);
                    }
                    Log($"      [SQL Server] -> TRẢ URL VỀ: Bảng [{sqlTable}] -> Cột [{sqlColumn}] (Bản ghi: {identifierInfo})");
                }
            }

            Log("=========================================================================");
            Log("CHI TIẾT ĐƯỜNG ĐI CỦA DỮ LIỆU VÀO SQL SERVER VÀ CLOUDINARY");
            Log("=========================================================================");

            Log("\n[1] BẢNG USERS");
            foreach (var u in blueprint.Users)
            {
                Log($"  - Dữ liệu Text: Username='{u.Username}', PasswordHash='{u.PasswordHash}', Email='{u.Email}', Role={u.SystemRole}");
                Log($"    => Lệnh SQL Mô phỏng: INSERT INTO [Users] (Id, Username, Email, FullName, SystemRole, Status, ...)");
            }

            Log("\n[2] BẢNG PAGES (VÀ PAGE_USER_ROLES)");
            foreach (var p in blueprint.Pages)
            {
                Log($"\n  * PAGE: '{p.Name}' (Slug: {p.Slug} | Loại: {p.Category})");
                Log($"    => Lệnh SQL Mô phỏng 1: INSERT INTO [Pages] (Id, Name, Slug, Bio, WebsiteUrl, OriginCountry, ...)");
                Log($"    => Lệnh SQL Mô phỏng 2: INSERT INTO [PageUserRoles] (Cấp quyền Owner cho UserId: {p.OwnerUserId})");
                SimulateCloudinary(p._AvatarUploadPlan, "Pages", "AvatarUrl", p.Name);
            }

            Log("\n[3] BẢNG MASTER RELEASES");
            foreach (var m in blueprint.MasterReleases)
            {
                Log($"\n  * MASTER RELEASE: '{m.Title}' (Năm: {m.ReleaseYear})");
                Log($"    => Lệnh SQL Mô phỏng: INSERT INTO [MasterReleases] (Id, Title, ReleaseYear, ArtistPageId, CreatedByUserId, ...)");
                SimulateCloudinary(m._CoverUploadPlan, "MasterReleases", "CoverImageUrl", m.Title);
            }

            Log("\n[4] BẢNG RELEASES & RELEASE_IDENTIFIERS");
            foreach (var r in blueprint.Releases)
            {
                Log($"\n  * RELEASE: '{r.Title}' (Edition: {r.Edition} | Format: {r.Format} | Price: {r.Price}đ | Digital Price: {r.DigitalPrice}đ)");
                Log($"    => Lệnh SQL Mô phỏng: INSERT INTO [Releases] (Id, Title, Edition, Format, MasterReleaseId, OwnedByPageId, ...)");
            }
            Log($"  => Đồng thời chèn {blueprint.ReleaseIdentifiers.Count} mã Barcode/Catalog vào bảng [ReleaseIdentifiers].");

            Log("\n[5] BẢNG RELEASE IMAGES (ẢNH BÌA, GALLERY, MATRIX)");
            foreach (var img in blueprint.ReleaseImages)
            {
                if (img.Category == ImageCategory.Matrix)
                {
                    Log($"  * ẢNH {img.Category} cho Release ID [{img.ReleaseId}]");
                    Log($"    => Lệnh SQL Mô phỏng: INSERT INTO [ReleaseImages] (Category, ImageUrl) VALUES ('Matrix', '') -> LƯU RỖNG ĐỂ GIỮ CHỖ!");
                }
                else
                {
                    SimulateCloudinary(img._ImageUploadPlan, "ReleaseImages", "ImageUrl", $"Loại {img.Category} của Release {img.ReleaseId}");
                }
            }

            Log("\n[6] BẢNG TRACKS (AUDIO SỐ)");
            foreach (var t in blueprint.Tracks)
            {
                Log($"\n  * TRACK: '{t.Title}' (Vị trí: {t.Position} | Explicit: {t.IsExplicit} | Price: {t.Price}đ | Thời lượng: {t.DurationSeconds}s)");
                Log($"    => Lệnh SQL Mô phỏng: INSERT INTO [Tracks] (Id, Title, Position, IsExplicit, Price, DurationSeconds, ReleaseId)");
                SimulateCloudinary(t._AudioUploadPlan, "Tracks", "OriginalFlacUrl", t.Title);
            }

            Log("\n=========================================================================");
            Log("                      KẾT LUẬN TÍNH TOÀN VẸN                             ");
            Log("=========================================================================");
            Log($"- [SỐ LƯỢNG SQL]: Sẽ chèn {blueprint.Users.Count} Users, {blueprint.Pages.Count} Pages, {blueprint.MasterReleases.Count} Masters, {blueprint.Releases.Count} Releases, {blueprint.Tracks.Count} Tracks.");
            Log($"- [CLOUDINARY MẠNG]: Sẽ GỌI THẬT API Upload {uniqueUploads.Count} lần (cho {uniqueUploads.Count} file độc bản).");
            Log($"- [CLOUDINARY CACHE]: Đã CỨU ĐƯỢC {duplicateCacheHits.Count} lần gọi API thừa thãi nhờ Cache nội bộ!");

            if (missingFilesCount > 0)
            {
                Log($"\n[CẢNH BÁO ĐỎ]: CÓ {missingFilesCount} FILE VẬT LÝ KHÔNG TỒN TẠI. HÃY MỞ FILE TEXT KIỂM TRA TỪ KHÓA '[LỖI FILE VẬT LÝ]'!", ConsoleColor.Red);
            }
            else
            {
                Log("\n[XANH CHÍN]: DỮ LIỆU ĐÃ SẠCH SẼ 100%. SẴN SÀNG CHẠY PHÍM 4 ĐỂ BƠM THẬT!", ConsoleColor.Green);
            }

            Log($"\n-> Sếp có thể mở file báo cáo siêu chi tiết tại: {reportPath}", ConsoleColor.Yellow);
        }

        public static async Task ExecuteBlueprintAsync(string connectionString, string cloudinaryUrl)
        {
            Console.Clear();
            Console.WriteLine("=== [PHASE 2] KÍCH HOẠT ĐỘNG CƠ BƠM MÂY & SQL (BẢN GIA CỐ) ===");

            if (!File.Exists(BLUEPRINT_PATH)) { Console.WriteLine("[!] Không tìm thấy Bản vẽ!"); return; }

            var blueprint = JsonSerializer.Deserialize<SeedingBlueprint>(await File.ReadAllTextAsync(BLUEPRINT_PATH), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (blueprint == null) return;

            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;
            using var context = new ApplicationDbContext(dbOptions);

            if (await context.Users.AnyAsync())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!] Database đã có dữ liệu. Vui lòng Drop DB trước!");
                Console.ResetColor();
                return;
            }

            var cloudinary = new CloudinaryService(new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { { "Cloudinary:Url", cloudinaryUrl } }).Build());

            var uploadCache = new Dictionary<string, string>();

            try
            {
                Console.WriteLine("-> 1. Bơm Users (Đã bao gồm PasswordHash)...");
                var usersToInsert = blueprint.Users.Select(bpU => new User
                {
                    Id = bpU.Id,
                    Username = bpU.Username,
                    PasswordHash = bpU.PasswordHash,
                    Email = bpU.Email,
                    FullName = bpU.FullName,
                    SystemRole = bpU.SystemRole,
                    Status = bpU.Status,
                    IsEmailVerified = bpU.IsEmailVerified,
                    IsTermsAccepted = bpU.IsTermsAccepted
                }).ToList();

                context.Users.AddRange(usersToInsert);
                await context.SaveChangesAsync();

                Console.WriteLine("-> 2. Bơm Pages & Quyền Owner...");
                foreach (var bpP in blueprint.Pages)
                {
                    string avatarUrl = string.Empty;
                    if (bpP._AvatarUploadPlan != null && File.Exists(bpP._AvatarUploadPlan.LocalFilePath))
                    {
                        var plan = bpP._AvatarUploadPlan;
                        if (!uploadCache.ContainsKey(plan.LocalFilePath))
                        {
                            try { uploadCache[plan.LocalFilePath] = await cloudinary.UploadMediaAsync(plan.LocalFilePath, plan.ExpectedPublicId, plan.TargetCloudinaryFolder, plan.ResourceType) ?? ""; }
                            catch (Exception ex) { Console.WriteLine($"   [LỖI CLOUDINARY] Avatar '{bpP.Name}': {ex.Message}"); uploadCache[plan.LocalFilePath] = ""; }
                        }
                        avatarUrl = uploadCache[plan.LocalFilePath];
                    }

                    context.Pages.Add(new Page { Id = bpP.Id, Name = bpP.Name, Slug = bpP.Slug, Category = bpP.Category, Bio = bpP.Bio, WebsiteUrl = bpP.WebsiteUrl, OriginCountry = bpP.OriginCountry, IsVerified = bpP.IsVerified, Status = bpP.Status, AvatarUrl = avatarUrl, CreatedByUserId = bpP.CreatedByUserId, LastUpdatedByUserId = bpP.LastUpdatedByUserId });

                    context.PageUserRoles.Add(new PageUserRole { PageId = bpP.Id, UserId = bpP.OwnerUserId, Role = PageRole.Owner });
                    if (bpP.ManagerUserId != Guid.Empty && bpP.ManagerUserId != bpP.OwnerUserId)
                        context.PageUserRoles.Add(new PageUserRole { PageId = bpP.Id, UserId = bpP.ManagerUserId, Role = PageRole.Manager });
                }
                await context.SaveChangesAsync();

                Console.WriteLine("-> 3. Bơm Master Releases...");
                foreach (var bpM in blueprint.MasterReleases)
                {
                    string coverUrl = string.Empty;
                    if (bpM._CoverUploadPlan != null && System.IO.File.Exists(bpM._CoverUploadPlan.LocalFilePath))
                    {
                        var plan = bpM._CoverUploadPlan;
                        if (!uploadCache.ContainsKey(plan.LocalFilePath))
                        {
                            try { uploadCache[plan.LocalFilePath] = await cloudinary.UploadMediaAsync(plan.LocalFilePath, plan.ExpectedPublicId, plan.TargetCloudinaryFolder, plan.ResourceType) ?? ""; }
                            catch (Exception ex) { Console.WriteLine($"   [LỖI CLOUDINARY] Cover '{bpM.Title}': {ex.Message}"); uploadCache[plan.LocalFilePath] = ""; }
                        }
                        coverUrl = uploadCache[plan.LocalFilePath];
                    }
                    context.MasterReleases.Add(new MasterRelease { Id = bpM.Id, Title = bpM.Title, ReleaseYear = bpM.ReleaseYear, CountryOfOrigin = bpM.CountryOfOrigin, Status = bpM.Status, CoverImageUrl = coverUrl, CreatedByUserId = bpM.CreatedByUserId, CreatedByPageId = bpM.CreatedByPageId, LastUpdatedByUserId = bpM.LastUpdatedByUserId, LastUpdatedByPageId = bpM.LastUpdatedByPageId });
                    context.MasterReleaseArtists.Add(new MasterReleaseArtist { MasterReleaseId = bpM.Id, ArtistPageId = bpM.ArtistPageId });
                }
                await context.SaveChangesAsync();

                Console.WriteLine("-> 4. Bơm Releases & Identifiers...");
                var releasesToInsert = blueprint.Releases.Select(bpR => new Release { Id = bpR.Id, MasterReleaseId = bpR.MasterReleaseId, OwnedByPageId = bpR.OwnedByPageId, Title = bpR.Title, Edition = bpR.Edition, Format = bpR.Format, ReleaseYear = bpR.ReleaseYear, Country = bpR.Country, Notes = bpR.Notes, Price = bpR.Price, DigitalPrice = bpR.DigitalPrice, StockQuantity = bpR.StockQuantity, Status = bpR.Status, CreatedByUserId = bpR.CreatedByUserId, CreatedByPageId = bpR.CreatedByPageId, LastUpdatedByUserId = bpR.LastUpdatedByUserId, LastUpdatedByPageId = bpR.LastUpdatedByPageId }).ToList();
                context.Releases.AddRange(releasesToInsert);

                var relArtistsToInsert = blueprint.Releases.Select(bpR => new ReleaseArtist { ReleaseId = bpR.Id, ArtistPageId = bpR.ArtistPageId }).ToList();
                context.ReleaseArtists.AddRange(relArtistsToInsert);

                var idensToInsert = blueprint.ReleaseIdentifiers.Select(iden => new ReleaseIdentifier { Id = iden.Id, ReleaseId = iden.ReleaseId, Category = iden.Category, Value = iden.Value, Description = iden.Description }).ToList();
                context.ReleaseIdentifiers.AddRange(idensToInsert);
                await context.SaveChangesAsync();

                Console.WriteLine("-> 5. Bơm Release Images...");
                foreach (var bpImg in blueprint.ReleaseImages)
                {
                    string imgUrl = string.Empty;
                    if (bpImg._ImageUploadPlan != null && System.IO.File.Exists(bpImg._ImageUploadPlan.LocalFilePath))
                    {
                        var plan = bpImg._ImageUploadPlan;
                        if (!uploadCache.ContainsKey(plan.LocalFilePath))
                        {
                            try { uploadCache[plan.LocalFilePath] = await cloudinary.UploadMediaAsync(plan.LocalFilePath, plan.ExpectedPublicId, plan.TargetCloudinaryFolder, plan.ResourceType) ?? ""; }
                            catch (Exception ex) { Console.WriteLine($"   [LỖI CLOUDINARY] Gallery Image: {ex.Message}"); uploadCache[plan.LocalFilePath] = ""; }
                        }
                        imgUrl = uploadCache[plan.LocalFilePath];
                    }
                    context.ReleaseImages.Add(new ReleaseImage { Id = bpImg.Id, ReleaseId = bpImg.ReleaseId, Category = bpImg.Category, ImageUrl = imgUrl });
                }
                await context.SaveChangesAsync();

                Console.WriteLine("-> 6. Bơm Tracks (KIỂM SOÁT AUDIO CACHE KHẮT KHE)...");
                var tracksToInsert = new List<Track>();
                foreach (var bpT in blueprint.Tracks)
                {
                    string audioUrl = string.Empty;
                    if (bpT._AudioUploadPlan != null && System.IO.File.Exists(bpT._AudioUploadPlan.LocalFilePath))
                    {
                        var plan = bpT._AudioUploadPlan;
                        if (!uploadCache.ContainsKey(plan.LocalFilePath))
                        {
                            Console.WriteLine($"      [Đang Upload] {Path.GetFileName(plan.LocalFilePath)}");
                            try { uploadCache[plan.LocalFilePath] = await cloudinary.UploadMediaAsync(plan.LocalFilePath, plan.ExpectedPublicId, plan.TargetCloudinaryFolder, plan.ResourceType) ?? ""; }
                            catch (Exception ex) { Console.WriteLine($"      [LỖI UPLOAD AUDIO] {bpT.Title}: {ex.Message}"); uploadCache[plan.LocalFilePath] = ""; }
                        }
                        else
                        {
                            Console.WriteLine($"      [Cache Hit - Bỏ Qua Upload] {Path.GetFileName(plan.LocalFilePath)}");
                        }
                        audioUrl = uploadCache[plan.LocalFilePath];
                    }
                    tracksToInsert.Add(new Track { Id = bpT.Id, ReleaseId = bpT.ReleaseId, Title = bpT.Title, Position = bpT.Position, DurationSeconds = bpT.DurationSeconds, IsExplicit = bpT.IsExplicit, Price = bpT.Price, OriginalFlacUrl = audioUrl });
                }
                context.Tracks.AddRange(tracksToInsert);
                await context.SaveChangesAsync();

                Console.WriteLine("\n[XUẤT SẮC] ĐỘNG CƠ BƠM ĐÃ HOÀN TẤT, BẢO VỆ TÀI NGUYÊN CLOUDINARY THÀNH CÔNG 100%!");
            }
            catch (Exception sysEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[HỆ THỐNG VĂNG LỖI]: {sysEx.Message}");
                if (sysEx.InnerException != null) Console.WriteLine($"[CHI TIẾT]: {sysEx.InnerException.Message}");
                Console.ResetColor();
            }
        }
    }

    public static class ScraperEngine
    {
        private const string USER_AGENT = "MusicEcom_DataLake_Bot/3.0";

        private const string BASE_DATALAKE = @"D:\DoAnThucTapCuoiKhoa\DataLake";
        private const string ARTISTS_DIR = BASE_DATALAKE + @"\Artists_And_Bands";
        private const string LABELS_DIR = BASE_DATALAKE + @"\Labels";

        private static readonly string[] TARGET_MASTERS = {
        "23684", "10362", "24047", "8883", "26647", "13814", "9130", "25303", "32261", "17820"
    };

        private static HashSet<string> _downloadedArtists = new HashSet<string>();
        private static HashSet<string> _downloadedLabels = new HashSet<string>();
        private static HttpClient _client = new HttpClient();

        public static async Task RunAsync(string discogsToken)
        {
            Console.Clear();
            Console.WriteLine("=== DEEP SCRAPER BOT (BỘ LỌC TÍN NHIỆM & CHỮ KÝ ĐỘC BẢN - V4) ===");
            Directory.CreateDirectory(ARTISTS_DIR);
            Directory.CreateDirectory(LABELS_DIR);

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Discogs", $"token={discogsToken}");
            foreach (var masterId in TARGET_MASTERS)
            {
                await ProcessMasterDeepAsync(masterId);
            }

            Console.WriteLine("\n[XUẤT SẮC] HOÀN THÀNH QUÉT DỮ LIỆU!");
        }

        static async Task ProcessMasterDeepAsync(string masterId)
        {
            Console.WriteLine($"\n=========================================");
            Console.WriteLine($"[MASTER] Đang cào Master ID: {masterId}");

            JObject? masterJson = await FetchApiJsonAsync($"https://api.discogs.com/masters/{masterId}");
            if (masterJson == null) return;

            string masterTitle = masterJson["title"]?.ToString() ?? "unknown_master";
            string mainArtistId = masterJson["artists"]?[0]?["id"]?.ToString() ?? "";
            string mainArtistName = masterJson["artists"]?[0]?["name"]?.ToString() ?? "unknown_artist";

            JObject? versionsJson = await FetchApiJsonAsync($"https://api.discogs.com/masters/{masterId}/versions");
            string releaseCategory = "Album";
            List<JToken> topReleases = new List<JToken>();

            if (versionsJson != null && versionsJson["versions"] is JArray versions && versions.Count > 0)
            {
                string firstFormat = versions[0]["format"]?.ToString() ?? "";
                if (firstFormat.Contains("Single")) releaseCategory = "Single";
                else if (firstFormat.Contains("EP")) releaseCategory = "EP";
                else if (firstFormat.Contains("Mixtape")) releaseCategory = "Mixtape";

                topReleases = PickTop5DiverseReleases(versions);
            }

            string masterDir = EnsureMasterDirectory(mainArtistName, releaseCategory, masterTitle);
            await File.WriteAllTextAsync(Path.Combine(masterDir, $"master_{masterId}.json"), masterJson.ToString());
            await DownloadAllImagesAsync(masterJson, masterDir, $"cover_master_{masterId}", $"cover_master_{masterId}");

            if (!string.IsNullOrEmpty(mainArtistId)) await ProcessArtistAsync(mainArtistId, mainArtistName);

            if (topReleases.Count > 0)
            {
                string releasesDir = Path.Combine(masterDir, "Releases");
                Directory.CreateDirectory(releasesDir);
                foreach (var rel in topReleases)
                {
                    string relId = rel["id"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(relId)) await ProcessReleaseDeepAsync(relId, releasesDir);
                }
            }
        }

        static async Task ProcessReleaseDeepAsync(string releaseId, string releasesDir)
        {
            JObject? relJson = await FetchApiJsonAsync($"https://api.discogs.com/releases/{releaseId}");
            if (relJson == null) return;

            var (formatName, editionName) = ParseFormatAndEdition(relJson);

            string country = SanitizeCloudinaryName(relJson["country"]?.ToString() ?? "unknown");
            string rawYear = relJson["year"]?.ToString() ?? relJson["released"]?.ToString() ?? "";
            string year = Extract4DigitYear(rawYear);

            string fileNameBase = $"release_{releaseId}_{formatName}_{editionName}_{year}_{country}";
            Console.WriteLine($"      -> [RELEASE CỰC PHẨM] {fileNameBase}");

            await File.WriteAllTextAsync(Path.Combine(releasesDir, $"{fileNameBase}.json"), relJson.ToString());
            await DownloadAllImagesAsync(relJson, releasesDir, $"{fileNameBase}_cover", $"{fileNameBase}_gallery");

            if (relJson["labels"] is JArray labels)
            {
                foreach (var lbl in labels)
                {
                    string labelId = lbl["id"]?.ToString() ?? "";
                    string labelName = lbl["name"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(labelId)) await ProcessLabelAsync(labelId, labelName);
                }
            }

            if (relJson["extraartists"] is JArray extraArtists)
            {
                foreach (var art in extraArtists)
                {
                    string artId = art["id"]?.ToString() ?? "";
                    string artName = art["name"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(artId)) await ProcessArtistAsync(artId, artName);
                }
            }
        }

        static List<JToken> PickTop5DiverseReleases(JArray versions)
        {
            List<JToken> selected = new List<JToken>();
            HashSet<string> selectedSignatures = new HashSet<string>();

            var validVersions = versions
                .Where(v =>
                {
                    string formatStr = v["format"]?.ToString() ?? "";
                    string countryStr = v["country"]?.ToString() ?? "";
                    string dateStr = v["released"]?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(formatStr) || string.IsNullOrWhiteSpace(countryStr) || countryStr == "Unknown") return false;
                    if (string.IsNullOrWhiteSpace(dateStr) || Extract4DigitYear(dateStr) == "0000") return false;
                    if (GetBasicFormatInfo(formatStr).formatName == "unknown_media") return false;

                    return true;
                })
                .OrderBy(v => Extract4DigitYear(v["released"]?.ToString() ?? ""))
                .ToList();

            bool TryAddVersion(JToken v, string tag)
            {
                if (selected.Count >= 5) return false;

                string formatStr = v["format"]?.ToString() ?? "";
                var (formatName, editionName) = GetBasicFormatInfo(formatStr);
                string country = SanitizeCloudinaryName(v["country"]?.ToString() ?? "");
                string year = Extract4DigitYear(v["released"]?.ToString() ?? "");

                string signature = $"{formatName}_{editionName}_{year}_{country}";

                if (!selectedSignatures.Contains(signature))
                {
                    selectedSignatures.Add(signature);
                    selected.Add(v);
                    Console.WriteLine($"      -> [CHỌN {tag}] release_{v["id"]}_{signature}");
                    return true;
                }
                return false;
            }

            var original = validVersions.FirstOrDefault(v => GetBasicFormatInfo(v["format"]?.ToString() ?? "").editionName == "original");
            if (original != null) TryAddVersion(original, "ORIGINAL");

            var repress = validVersions.FirstOrDefault(v => GetBasicFormatInfo(v["format"]?.ToString() ?? "").editionName == "repress");
            if (repress != null) TryAddVersion(repress, "REPRESS");

            var reissue = validVersions.FirstOrDefault(v => GetBasicFormatInfo(v["format"]?.ToString() ?? "").editionName == "reissue");
            if (reissue != null) TryAddVersion(reissue, "REISSUE");

            string[] mediaFormats = { "Vinyl", "CD", "Cassette" };
            foreach (var mf in mediaFormats)
            {
                if (selected.Count >= 5) break;
                if (!selected.Any(s => s["format"]?.ToString().Contains(mf) == true))
                {
                    var match = validVersions.FirstOrDefault(v => v["format"]?.ToString().Contains(mf) == true);
                    if (match != null) TryAddVersion(match, $"FORMAT_{mf.ToUpper()}");
                }
            }

            foreach (var v in validVersions)
            {
                if (selected.Count >= 5) break;
                TryAddVersion(v, "BỔ SUNG");
            }

            return selected;
        }

        static (string formatName, string editionName) GetBasicFormatInfo(string formatStr)
        {
            string fName = "unknown_media";
            string lowerFormat = formatStr.ToLower();

            if (lowerFormat.Contains("vinyl") || lowerFormat.Contains("lp") || lowerFormat.Contains("12\"") || lowerFormat.Contains("7\"")) fName = "vinyl";
            else if (lowerFormat.Contains("cd")) fName = "cd";
            else if (lowerFormat.Contains("cassette") || lowerFormat.Contains("mc")) fName = "cassette";

            string eName = "original";
            if (lowerFormat.Contains("repress")) eName = "repress";
            else if (lowerFormat.Contains("reissue") || lowerFormat.Contains("remaster") || lowerFormat.Contains("deluxe") || lowerFormat.Contains("anniversary")) eName = "reissue";

            return (fName, eName);
        }

        static (string formatName, string editionName) ParseFormatAndEdition(JObject relJson)
        {
            string formatName = "unknown_media";
            string editionName = "original";

            if (relJson["formats"] is JArray formats && formats.Count > 0)
            {
                var firstFormat = formats[0];
                string formatStr = firstFormat["name"]?.ToString() ?? "";

                if (formatStr.Contains("Vinyl") || formatStr.Contains("LP")) formatName = "vinyl";
                else if (formatStr.Contains("CD")) formatName = "cd";
                else if (formatStr.Contains("Cassette")) formatName = "cassette";

                if (firstFormat["descriptions"] is JArray descriptions)
                {
                    string[] descArr = descriptions.Select(d => d.ToString().ToLower()).ToArray();
                    if (descArr.Contains("repress")) editionName = "repress";
                    else if (descArr.Contains("reissue") || descArr.Contains("remastered") || descArr.Contains("deluxe edition") || descArr.Contains("anniversary"))
                    {
                        editionName = "reissue";
                    }
                }
            }
            return (formatName, editionName);
        }

        static string Extract4DigitYear(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return "0000";
            var match = Regex.Match(dateStr, @"\b(19|20)\d{2}\b");
            return match.Success ? match.Value : "0000";
        }

        static async Task DownloadAllImagesAsync(JObject json, string targetDir, string primaryName, string secondaryPrefix)
        {
            if (json["images"] is not JArray images || images.Count == 0) return;

            int secondaryCount = 1;
            foreach (var img in images)
            {
                string imgUrl = img["resource_url"]?.ToString() ?? "";
                string type = img["type"]?.ToString().ToLower() ?? "secondary";
                if (string.IsNullOrEmpty(imgUrl)) continue;

                string fileName = type == "primary" ? $"{primaryName}.jpg" : $"{secondaryPrefix}_{secondaryCount++}.jpg";
                string filePath = Path.Combine(targetDir, fileName);

                if (File.Exists(filePath)) continue;

                await Task.Delay(1000);
                try
                {
                    byte[] imageBytes = await _client.GetByteArrayAsync(imgUrl);
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                }
                catch { }
            }
        }

        static async Task ProcessArtistAsync(string artistId, string artistName)
        {
            if (_downloadedArtists.Contains(artistId)) return;
            _downloadedArtists.Add(artistId);

            JObject? artJson = await FetchApiJsonAsync($"https://api.discogs.com/artists/{artistId}");
            if (artJson == null) return;

            string artistDir = Path.Combine(ARTISTS_DIR, SanitizeCloudinaryName(artistName));
            Directory.CreateDirectory(artistDir);

            await File.WriteAllTextAsync(Path.Combine(artistDir, $"artist_{artistId}.json"), artJson.ToString());
            await DownloadAllImagesAsync(artJson, artistDir, $"avatar_{artistId}", $"avatar_{artistId}");
        }

        static async Task ProcessLabelAsync(string labelId, string labelName)
        {
            if (_downloadedLabels.Contains(labelId)) return;
            _downloadedLabels.Add(labelId);

            JObject? lblJson = await FetchApiJsonAsync($"https://api.discogs.com/labels/{labelId}");
            if (lblJson == null) return;

            string labelDir = Path.Combine(LABELS_DIR, SanitizeCloudinaryName(labelName));
            Directory.CreateDirectory(labelDir);

            await File.WriteAllTextAsync(Path.Combine(labelDir, $"label_{labelId}.json"), lblJson.ToString());
            await DownloadAllImagesAsync(lblJson, labelDir, $"logo_{labelId}", $"logo_{labelId}");
        }

        static async Task<JObject?> FetchApiJsonAsync(string url)
        {
            int retryCount = 0;
            while (retryCount < 3)
            {
                await Task.Delay(1000);
                try
                {
                    var response = await _client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return JObject.Parse(await response.Content.ReadAsStringAsync());
                    }
                    else if ((int)response.StatusCode == 429)
                    {
                        Console.WriteLine($"      [API RATE LIMIT] Discogs tạm khóa IP. Đang chờ 60 giây...");
                        await Task.Delay(60000);
                        retryCount++;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"      [LỖI API] Status: {response.StatusCode}");
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        static string EnsureMasterDirectory(string artistName, string releaseCategory, string masterTitle)
        {
            string artistDir = Path.Combine(ARTISTS_DIR, SanitizeCloudinaryName(artistName));
            string categoryPath = Path.Combine(artistDir, releaseCategory + "s");
            string masterPath = Path.Combine(categoryPath, SanitizeCloudinaryName(masterTitle));

            Directory.CreateDirectory(Path.Combine(masterPath, "Releases"));
            return masterPath;
        }

        static string SanitizeCloudinaryName(string name)
        {
            int bracketIndex = name.IndexOf('(');
            if (bracketIndex > 0) name = name.Substring(0, bracketIndex).Trim();
            name = name.ToLower().Replace(" ", "_").Replace("-", "_");
            return Regex.Replace(name, @"[^a-z0-9_]", "");
        }
    }
}