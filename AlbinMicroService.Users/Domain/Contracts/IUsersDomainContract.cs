using AlbinMicroService.Users.Domain.DTOs;

namespace AlbinMicroService.Users.Domain.Contracts
{
    public interface IUsersDomainContract
    {
        /// <summary>
        /// Validates the UserDto
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>The ValidatorTemplate instance.</returns>
        ValidatorTemplate ValidateUserDto(UserDto userDto);
        /// <summary>
        /// Hashes the user password
        /// </summary>
        /// <param name="userPassword"></param>
        /// <returns>The Hashed User Password as String.</returns>
        string HashUserPassword(string userPassword);
        /// <summary>
        /// Sends a welcome email to the user
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="receiverUsername"></param>
        /// <returns>Whether email sended or not.</returns>
        Task<bool> SendWelcomeEmailToUser(string toEmail, string receiverUsername);
        Task<bool> VerifyUsernameAndEmailNotExists(string email, string username);
    }
}
