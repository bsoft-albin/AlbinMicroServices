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
    }
}
