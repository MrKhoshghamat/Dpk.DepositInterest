using Autofac;
using Dpk.DepositInterest.Modules.Administration.Domain.Users;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Users;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Authentication
{
    internal class AuthenticationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserContext>()
                .As<IUserContext>()
                .InstancePerLifetimeScope();
        }
    }
}