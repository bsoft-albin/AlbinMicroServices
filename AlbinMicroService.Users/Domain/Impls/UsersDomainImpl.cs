using AlbinMicroService.Users.Domain.Contracts;
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

            return _dynamicMeths.HashStringFullEncoded(userPassword);
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

            EmailTemplate emailTemplate = new(WebAppSettings.Settings.Email.SmtpPort, WebAppSettings.Settings.Email.SmtpServer, WebAppSettings.Settings.Email.FromEmail, WebAppSettings.Settings.Email.EmailPassword, toEmail)
            {
                Title = "AlbinMicroServices Inc",
                Subject = "Welcome to AlbinMicroServices",
                Username = receiverUsername,
                Body = $"<h1>Welcome {receiverUsername},</h1><p>Thank you for registering with us.</p>"
            };

            return await _dynamicMeths.SendEmailAsync(emailTemplate);
        }

        public ValidatorTemplate ValidateUserDto(UserRegisterDto userDto)
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
            return await _usersInfra.CheckUsernameExistsOrNotInfraAsync(username) > 0;
        }

        public async Task<bool> VerifyEmailExistsOrNotAsync(string email)
        {
            return await _usersInfra.CheckEmailExistsOrNotInfraAsync(email) > 0;
        }
    }
}
