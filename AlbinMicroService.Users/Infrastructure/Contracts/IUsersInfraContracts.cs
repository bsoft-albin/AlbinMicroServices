namespace AlbinMicroService.Users.Infrastructure.Contracts
{
    public interface IUsersInfraContracts
    {
        Task<short> CheckUsernameExistsOrNotInfraAsync(string username);
    }
}
