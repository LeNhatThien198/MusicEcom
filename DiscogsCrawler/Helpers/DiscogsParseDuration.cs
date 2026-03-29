using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscogsCrawler.Helpers
{
    public class DiscogsParseDuration
    {
        public static int ParseDuration(string durationStr)
        {
            if (string.IsNullOrWhiteSpace(durationStr)) return 0;
            var parts = durationStr.Split(':');

            if (parts.Length == 2 && int.TryParse(parts[0], out int m) && int.TryParse(parts[1], out int s))
                return m * 60 + s;

            if (parts.Length == 3 && int.TryParse(parts[0], out int h) && int.TryParse(parts[1], out int m2) && int.TryParse(parts[2], out int s2))
                return h * 3600 + m2 * 60 + s2;

            return 0;
        }
    }
}
