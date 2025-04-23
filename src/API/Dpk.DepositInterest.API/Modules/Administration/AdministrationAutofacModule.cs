using Autofac;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Dpk.DepositInterest.Modules.Administration.Infrastructure;

namespace Dpk.DepositInterest.API.Modules.Administration
{
    internal class AdministrationAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdministrationModule>()
                .As<IAdministrationModule>()
                .InstancePerLifetimeScope();
        }
    }
}