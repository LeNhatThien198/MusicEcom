namespace Backend_API.Helpers
{
    public class MediaHelper
    {
        public static int GetActualAudioDuration(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return 0;
            try
            {
                using var tfile = TagLib.File.Create(filePath);
                return (int)tfile.Properties.Duration.TotalSeconds;
            }
            catch { return 0; }
        }
    }
}
