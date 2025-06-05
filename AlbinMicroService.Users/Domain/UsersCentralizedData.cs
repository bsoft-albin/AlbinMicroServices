namespace AlbinMicroService.Users.Domain
{
    public abstract class UsersActionNames
    {
        public const string RegisterUser = "register-user";
    }

    public static class UsersSqlQueries
    {
        public static string UsernameExistCount => "SELECT COUNT(Id) as UsernameCounts FROM Users WHERE Username = @username;";
        public static string UserRoleGet => "SELECT ur.Role FROM users u INNER JOIN user_roles ur ON u.Id = ur.UserId WHERE Username = @username AND IsDeleted = 1 AND IsActive = 1 AND IsVerified = 1;";
    }
}
