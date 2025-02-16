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
    }
}
