namespace HearUS.Domain.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";

        public const string Expert = "Expert";

        public const string Member = "Member";

        public static readonly string[] All = { Admin, Expert, Member };
    }

    public static class AppPolicies
    {
        public const string ExpertsOnly = "ExpertsOnly";
    }
}
