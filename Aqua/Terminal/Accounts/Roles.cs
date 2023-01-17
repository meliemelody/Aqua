namespace Aqua.Terminal.Accounts
{
    public enum Roles
    {
        System = 0,
        Admin = 1,
        User = 2,
        Guest = 3
    }

    public static class RolesUtil
    {
        public static string RoleToString(Roles role)
        {
            switch (role)
            {
                case Roles.System:
                    return "System";

                case Roles.Admin:
                    return "Admin";

                case Roles.User:
                    return "User";

                case Roles.Guest:
                    return "Guest";

                default:
                    return "Unknown";
            }
        }
    }
}
