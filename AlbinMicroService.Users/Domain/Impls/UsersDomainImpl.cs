using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.Models.Dtos;
using AlbinMicroService.Users.Domain.Validator;
using AlbinMicroService.Users.Infrastructure.Contracts;
using FluentValidation.Results;

namespace AlbinMicroService.Users.Domain.Impls
{
    public class UsersDomainImpl(IDynamicMeths _dynamicMeths, IUsersInfraContract _usersInfra) : IUsersDomainContract
    {
        public string HashUserPassword(string userPassword)
        {
            if (string.IsNullOrWhiteSpace(userPassword))
            {
                throw new ArgumentNullException(nameof(userPassword), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(nameof(userPassword)));
            }

            return _dynamicMeths.HashString(userPassword);
        }

        public async Task<bool> SendWelcomeEmailToUserAsync(string toEmail, string receiverUsername)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                throw new ArgumentNullException(nameof(toEmail), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(nameof(toEmail)));
            }
            if (string.IsNullOrWhiteSpace(receiverUsername))
            {
                throw new ArgumentNullException(nameof(receiverUsername), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(nameof(receiverUsername)));
            }

            EmailTemplate emailTemplate = new(WebAppConfigs.Settings.Email.SmtpPort, WebAppConfigs.Settings.Email.SmtpServer, WebAppConfigs.Settings.Email.FromEmail, WebAppConfigs.Settings.Email.EmailPassword, toEmail)
            {
                Title = "AlbinMicroServices Inc",
                Subject = "Welcome to AlbinMicroServices",
                Username = receiverUsername,
                Body = $"<h1>Welcome {receiverUsername},</h1><p>Thank you for registering with us.</p>"
            };

            return await _dynamicMeths.SendEmailAsync(emailTemplate);
        }

        public ValidatorTemplate ValidateUserDto(UserDto userDto)
        {
            UserDtoValidator validator = new();
            ValidatorTemplate validatorTemplate = new();
            ValidationResult result = validator.Validate(userDto);

            if (!result.IsValid)
            {
                foreach (ValidationFailure error in result.Errors)
                {
                    validatorTemplate.Errors.Add($"{error.PropertyName} - {error.ErrorMessage}");
                }
            }
            else
            {
                validatorTemplate.IsValidated = true;
            }

            return validatorTemplate;
        }

        public async Task<bool> VerifyUsernameExistsOrNotAsync(string username)
        {
            if (await _usersInfra.CheckUsernameExistsOrNotInfraAsync(username) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
