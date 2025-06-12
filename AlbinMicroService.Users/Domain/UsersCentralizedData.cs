namespace AlbinMicroService.Users.Domain
{
    public abstract class UsersActionNames
    {
        public const string RegisterUser = "register-user";
    }

    /// <summary>
    /// Provides SQL query strings for operations related to user data in the database.
    /// </summary>
    /// <remarks>This class contains predefined SQL queries for common user-related operations, such as
    /// checking  if a username exists or retrieving a user's role. These queries are intended to be used with 
    /// parameterized commands to prevent SQL injection.</remarks>
    public static class UsersSqlQueries
    {
        public const string UsernameExistCount = "SELECT COUNT(Id) as UsernameCounts FROM Users WHERE Username = @username;";
        public const string UserRoleGet = "SELECT ur.Role FROM users u INNER JOIN user_roles ur ON u.Id = ur.UserId WHERE Username = @username AND IsDeleted = 1 AND IsActive = 1 AND IsVerified = 1;";
    }
}
