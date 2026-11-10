namespace HearUS.Helpers
{
    public static class ArabicCount
    {
        public static string Comments(int count) => count switch
        {
            0 => "لا تعليقات",
            1 => "تعليق واحد",
            2 => "تعليقان",
            <= 10 => $"{count} تعليقات",
            _ => $"{count} تعليقاً"
        };
    }
}
