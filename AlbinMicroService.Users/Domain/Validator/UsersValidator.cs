using AlbinMicroService.Users.Domain.DTOs;
using FluentValidation;

namespace AlbinMicroService.Users.Domain.Validator
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(user => user.IsActive)
                .NotNull().WithMessage("IsActive field is required.");

            RuleFor(user => user.CreatedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

            RuleFor(user => user.UpdatedAt)
                .GreaterThanOrEqualTo(user => user.CreatedAt).WithMessage("UpdatedAt cannot be earlier than CreatedAt.");

            RuleFor(user => user.DeletedAt)
                .GreaterThan(user => user.CreatedAt).When(user => user.DeletedAt.HasValue)
                .WithMessage("DeletedAt must be after CreatedAt.");
        }
    }
}
