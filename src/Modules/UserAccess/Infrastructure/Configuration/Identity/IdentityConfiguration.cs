using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.IdentityServer;
using Duende.IdentityServer.Validation;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Identity;

public static class IdentityConfiguration
{
    public static IServiceCollection ConfigureIdentityService(this IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
            .AddInMemoryApiResources(IdentityServerConfig.GetApis())
            .AddInMemoryClients(IdentityServerConfig.GetClients())
            .AddInMemoryPersistedGrants()
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();

        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, x =>
            {
                x.Authority = "http://localhost:5000";
                x.ApiName = "myMeetingsAPI";
                x.RequireHttpsMetadata = false;
            });

        return services;
    }

    public static IApplicationBuilder AddIdentityService(this IApplicationBuilder app)
    {
        app.UseIdentityServer();
        return app;
    }
}
