namespace AlbinMicroService.Users.Domain.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }
        public required string Username { get; set; } // it enforces initialization at object creation
        public required string Password { get; set; } // it enforces initialization at object creation
        public required string Email { get; set; } // it enforces initialization at object creation
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class UserProfileDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
    }

    public class UserAddressDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class UserActivityLogDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public DateTime? ActivityDate { get; set; }
    }

    public class UserRoleDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string? Permissions { get; set; }
    }

    public class UserSecurityDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? VerificationToken { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiresAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class UserSocialAuthDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
    }

}
