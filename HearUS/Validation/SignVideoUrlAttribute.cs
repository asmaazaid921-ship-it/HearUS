using System.ComponentModel.DataAnnotations;
using HearUS.Application.Common.Video;

namespace HearUS.Validation
{
    public sealed class SignVideoUrlAttribute : ValidationAttribute
    {
        public SignVideoUrlAttribute()
            : base("أدخل رابط YouTube أو Vimeo أو فيديو مباشر (mp4/webm/ogg) يبدأ بـ https.")
        {
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            var text = value.ToString();
            return string.IsNullOrWhiteSpace(text) || VideoEmbed.IsSupported(text);
        }
    }
}
