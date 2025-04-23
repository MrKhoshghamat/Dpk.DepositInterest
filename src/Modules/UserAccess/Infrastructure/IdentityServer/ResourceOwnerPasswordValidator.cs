using Dpk.DepositInterest.Modules.UserAccess.Application.Authentication.Authenticate;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace Dpk.DepositInterest.Modules.UserAccess.Infrastructure.IdentityServer
{
    internal class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserAccessModule _userAccessModule;

        public ResourceOwnerPasswordValidator(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var authenticationResult = await _userAccessModule.ExecuteCommandAsync(
                new AuthenticateCommand(context.UserName, context.Password));

            if (!authenticationResult.IsAuthenticated)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant,
                    authenticationResult.AuthenticationError);

                return;
            }

            context.Result = new GrantValidationResult(
                authenticationResult.User.Id.ToString(),
                "forms",
                authenticationResult.User.Claims);
        }
    }
}