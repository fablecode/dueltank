using System.Text.RegularExpressions;
using dueltank.core.Constants;

namespace dueltank.Domain.Helpers
{
    public static class YoutubeHelpers
    {
        public static string ExtractVideoId(string videoUrl)
        {
            var idMatch = new Regex(YoutubeConstants.UrlRegexExpression).Match(videoUrl);

            return idMatch.Success ? idMatch.Groups[1].Value : null;
        }
    }
}