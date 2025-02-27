using AlbinMicroService.Users.Domain.Contracts;
using AlbinMicroService.Users.Domain.DTOs;
using AlbinMicroService.Users.Domain.Validator;
using FluentValidation.Results;

namespace AlbinMicroService.Users.Domain.Impls
{
    public class UsersDomainImpl(IDynamicMeths _dynamicMeths) : IUsersDomainContract
    {
        public string HashUserPassword(string userPassword)
        {
            return _dynamicMeths.HashString(userPassword);
        }

        public async Task<bool> SendWelcomeEmailToUser(string toEmail, string receiverUsername)
        {
            if (string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(receiverUsername))
            {
                throw new ArgumentNullException("toEmail or receiverUsername cannot be null or empty.");
            }

            EmailTemplate emailTemplate = new(WebAppConfigs.Settings.Email.SmtpPort, WebAppConfigs.Settings.Email.SmtpServer, WebAppConfigs.Settings.Email.FromEmail, WebAppConfigs.Settings.Email.EmailPassword, toEmail);
            emailTemplate.Title = "AlbinMicroServices Inc";
            emailTemplate.Subject = "Welcome to AlbinMicroService";
            emailTemplate.Username = receiverUsername;
            emailTemplate.Body = $"<h1>Welcome {receiverUsername},</h1><p>Thank you for registering with us.</p>";

            bool response = await _dynamicMeths.SendEmailAsync(emailTemplate);

            return response;
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

        public Task<bool> VerifyUsernameExists(string email, string username)
        {
            throw new NotImplementedException();
        }
    }
}
