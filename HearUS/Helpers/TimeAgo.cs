namespace HearUS.Helpers
{
    public static class TimeAgo
    {
        public static string Format(DateTime utcTime)
        {
            var value = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
            var diff = DateTime.UtcNow - value;

            if (diff.TotalSeconds < 60)
            {
                return "الآن";
            }

            if (diff.TotalMinutes < 60)
            {
                return "منذ " + Plural((int)diff.TotalMinutes, "دقيقة", "دقيقتين", "دقائق", "دقيقة");
            }

            if (diff.TotalHours < 24)
            {
                return "منذ " + Plural((int)diff.TotalHours, "ساعة", "ساعتين", "ساعات", "ساعة");
            }

            if (diff.TotalDays < 7)
            {
                return "منذ " + Plural((int)diff.TotalDays, "يوم", "يومين", "أيام", "يوم");
            }

            if (diff.TotalDays < 30)
            {
                return "منذ " + Plural((int)(diff.TotalDays / 7), "أسبوع", "أسبوعين", "أسابيع", "أسبوع");
            }

            if (diff.TotalDays < 365)
            {
                return "منذ " + Plural((int)(diff.TotalDays / 30), "شهر", "شهرين", "أشهر", "شهر");
            }

            return "منذ " + Plural((int)(diff.TotalDays / 365), "سنة", "سنتين", "سنوات", "سنة");
        }

        private static string Plural(int count, string one, string two, string few, string many)
        {
            return count switch
            {
                1 => one,
                2 => two,
                >= 3 and <= 10 => $"{count} {few}",
                _ => $"{count} {many}"
            };
        }
    }
}
