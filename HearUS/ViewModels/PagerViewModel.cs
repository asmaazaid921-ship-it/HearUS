namespace HearUS.ViewModels
{
    public class PagerViewModel
    {
        public int Page { get; set; } = 1;

        public int TotalPages { get; set; }

        public string Area { get; set; } = string.Empty;

        public string Controller { get; set; } = "Home";

        public string Action { get; set; } = "Index";

        public Dictionary<string, string> RouteValues { get; set; } = new();
    }
}
