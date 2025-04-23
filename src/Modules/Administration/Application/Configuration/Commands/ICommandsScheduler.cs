using Dpk.DepositInterest.Modules.Administration.Application.Contracts;

namespace Dpk.DepositInterest.Modules.Administration.Application.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);

        Task EnqueueAsync<T>(ICommand<T> command);
    }
}