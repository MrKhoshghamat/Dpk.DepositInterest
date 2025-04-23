using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace Dpk.DepositInterest.Modules.UserAccess.Infrastructure.IdentityServer
{
    internal class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims.Where(x => x.Type == CustomClaimTypes.Roles).ToList());
            context.IssuedClaims.Add(context.Subject.Claims.Single(x => x.Type == CustomClaimTypes.Name));
            context.IssuedClaims.Add(context.Subject.Claims.Single(x => x.Type == CustomClaimTypes.Email));
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(context.IsActive);
        }
    }
}