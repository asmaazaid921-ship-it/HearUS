using System.Text.RegularExpressions;

namespace HearUS.Application.Common.Video
{
    public sealed class VideoEmbed
    {
        private static readonly Regex YouTubeIdPattern = new("^[A-Za-z0-9_-]{11}$", RegexOptions.Compiled);
        private static readonly Regex VimeoIdPattern = new("^\\d{6,12}$", RegexOptions.Compiled);

        public string? EmbedUrl { get; private init; }

        public string? FileUrl { get; private init; }

        public string? ExternalUrl { get; private init; }

        public static VideoEmbed? Parse(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            if (!Uri.TryCreate(raw.Trim(), UriKind.Absolute, out var uri) ||
                uri.Scheme != Uri.UriSchemeHttps)
            {
                return null;
            }

            var host = uri.Host.ToLowerInvariant();
            var externalUrl = uri.ToString();

            if (host is "youtube.com" or "www.youtube.com" or "m.youtube.com" or "youtu.be")
            {
                var id = ExtractYouTubeId(uri, host);
                if (id is not null)
                {
                    return new VideoEmbed
                    {
                        EmbedUrl = $"https://www.youtube-nocookie.com/embed/{id}",
                        ExternalUrl = externalUrl
                    };
                }
            }

            if (host is "vimeo.com" or "www.vimeo.com" or "player.vimeo.com")
            {
                var id = ExtractVimeoId(uri);
                if (id is not null)
                {
                    return new VideoEmbed
                    {
                        EmbedUrl = $"https://player.vimeo.com/video/{id}",
                        ExternalUrl = externalUrl
                    };
                }
            }

            var extension = Path.GetExtension(uri.AbsolutePath).ToLowerInvariant();
            if (extension is ".mp4" or ".webm" or ".ogg")
            {
                return new VideoEmbed
                {
                    FileUrl = externalUrl,
                    ExternalUrl = externalUrl
                };
            }

            return null;
        }

        public static bool IsSupported(string? raw) => Parse(raw) is not null;

        private static string? ExtractYouTubeId(Uri uri, string host)
        {
            if (host == "youtu.be")
            {
                var pathId = uri.AbsolutePath.Trim('/');
                return IsYouTubeId(pathId) ? pathId : null;
            }

            var segments = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2 &&
                (segments[0] is "embed" or "shorts" or "live") &&
                IsYouTubeId(segments[1]))
            {
                return segments[1];
            }

            var queryId = GetQueryValue(uri.Query, "v");
            return queryId is not null && IsYouTubeId(queryId) ? queryId : null;
        }

        private static string? ExtractVimeoId(Uri uri)
        {
            var segments = uri.AbsolutePath.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length == 0)
            {
                return null;
            }

            var candidate = segments[^1];
            return VimeoIdPattern.IsMatch(candidate) ? candidate : null;
        }

        private static string? GetQueryValue(string query, string key)
        {
            foreach (var pair in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = pair.Split('=', 2);
                if (parts.Length == 2 && parts[0] == key)
                {
                    return Uri.UnescapeDataString(parts[1]);
                }
            }

            return null;
        }

        private static bool IsYouTubeId(string value) => YouTubeIdPattern.IsMatch(value);
    }
}
