using Dpk.DepositInterest.BuildingBlocks.Infrastructure;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.InternalCommands;
using Dpk.DepositInterest.Modules.Administration.Application.Configuration.Commands;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing
{
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private readonly AdministrationContext _administrationContext;
        private readonly ICommandHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated,
            IUnitOfWork unitOfWork,
            AdministrationContext administrationContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _administrationContext = administrationContext;
        }

        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            await _decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                InternalCommand internalCommand = await _administrationContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}