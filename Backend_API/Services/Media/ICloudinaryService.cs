namespace Backend_API.Services.Media
{
    public interface ICloudinaryService
    {
        Task<string?> UploadMediaAsync(string filePath, string cloudinaryPublicId, string targetFolder, string resourceType = "image");
    }
}