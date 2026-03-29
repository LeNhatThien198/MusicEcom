using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Backend_API.Services.Media
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            string cloudinaryUrl = config["Cloudinary:Url"]
                ?? throw new ArgumentNullException("CLOUDINARY_URL is missing in appsettings.");

            _cloudinary = new Cloudinary(cloudinaryUrl);
            _cloudinary.Api.Secure = true; 
        }

        public async Task<string?> UploadMediaAsync(string filePath, string cloudinaryPublicId, string targetFolder, string resourceType = "image")
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;

            string publicId = cloudinaryPublicId.ToString();
            UploadResult result;

            if (resourceType.Equals("image", StringComparison.OrdinalIgnoreCase))
            {
                var imgParams = new ImageUploadParams()
                {
                    File = new FileDescription(filePath),
                    Folder = targetFolder,
                    PublicId = publicId,
                    Overwrite = true
                };
                result = await _cloudinary.UploadAsync(imgParams);
            }
            else 
            {
                var audioParams = new VideoUploadParams()
                {
                    File = new FileDescription(filePath),
                    Folder = targetFolder,
                    PublicId = publicId,
                    Overwrite = true
                };
                result = await _cloudinary.UploadAsync(audioParams);
            }

            if (result == null || result.Error != null || result.SecureUrl == null)
            {
                return null;
            }

            return result.SecureUrl.ToString();
        }
    }
}