using Slugify;

namespace Backend_API.Helpers
{
    public class StringHelper
    {
        public static string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Guid.NewGuid().ToString().Substring(0, 8);

            var config = new SlugHelperConfiguration();
            var helper = new SlugHelper(config);

            return helper.GenerateSlug(name);
        }
    }
}
