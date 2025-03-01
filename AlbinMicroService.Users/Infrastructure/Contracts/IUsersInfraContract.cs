namespace AlbinMicroService.Users.Infrastructure.Contracts
{
    public interface IUsersInfraContract
    {
        Task<short> CheckUsernameExistsOrNotInfraAsync(string username);
    }
}
