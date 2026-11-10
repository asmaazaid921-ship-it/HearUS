using HearUS.Domain.Entities;

namespace HearUS.Helpers
{
    public static class SignCategoryExtensions
    {
        public static string ToArabicName(this SignCategory category) => category switch
        {
            SignCategory.Alphabet => "الأبجدية",
            SignCategory.Numbers => "الأرقام",
            SignCategory.DailyLife => "الحياة اليومية",
            SignCategory.Health => "الصحة",
            SignCategory.Work => "العمل",
            SignCategory.Emotions => "المشاعر",
            _ => "أخرى"
        };
    }
}
