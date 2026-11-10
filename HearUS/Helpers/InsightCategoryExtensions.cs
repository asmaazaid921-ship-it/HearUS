using HearUS.Domain.Entities;

namespace HearUS.Helpers
{
    public static class InsightCategoryExtensions
    {
        public static string ToArabicName(this InsightCategory category)
        {
            return category switch
            {
                InsightCategory.Experience => "تجربة",
                InsightCategory.Awareness => "توعية",
                InsightCategory.SuccessStory => "قصة نجاح",
                InsightCategory.Guidance => "إرشاد",
                _ => category.ToString()
            };
        }
    }
}
