namespace HearUS.Areas.Account.ViewModels
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();

        public int PostsCount { get; set; }

        public int InsightsCount { get; set; }
    }
}
