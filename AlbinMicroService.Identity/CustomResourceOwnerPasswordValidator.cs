using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace AlbinMicroService.Identity
{
    public class CustomResourceOwnerPasswordValidator(IUserService userService) : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // Db Fetching...
            var user = await userService.ValidateCredentialsAsync(context.UserName, context.Password);

            if (user != null)
            {
                context.Result = new GrantValidationResult(
                    subject: user.Id,
                    authenticationMethod: "custom",
                    claims:
                    [
                        new Claim("username", user.Username),
                        new Claim("role", user.Role),
                        new Claim("userid", user.Id)
                    ]);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
            }
        }
    }
}
