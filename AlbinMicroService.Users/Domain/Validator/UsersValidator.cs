using FluentValidation;

namespace AlbinMicroService.Users.Domain.Validator
{
    public class UserDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.Role).NotEmpty().WithMessage("Role is required.");
        }
    }
}
