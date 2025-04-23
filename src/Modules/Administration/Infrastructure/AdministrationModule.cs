using Autofac;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing;
using MediatR;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure
{
    public class AdministrationModule : IAdministrationModule
    {
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            return await CommandsExecutor.Execute(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            await CommandsExecutor.Execute(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using (var scope = AdministrationCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                return await mediator.Send(query);
            }
        }
    }
}