namespace AlbinMicroService.Users.Domain
{
    public abstract class UsersActionNames
    {
        public const string RegisterUser = "register-user";
    }

    public static class UsersSqlQueries
    {
        public static string UsernameExistCount => "SELECT COUNT(Id) as UsernameCounts FROM Users WHERE Username = @username;";
    }
}
