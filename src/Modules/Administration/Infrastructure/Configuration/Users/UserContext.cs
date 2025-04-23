using Dpk.DepositInterest.BuildingBlocks.Application;
using Dpk.DepositInterest.Modules.Administration.Domain.Users;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Users
{
    internal class UserContext : IUserContext
    {
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public UserContext(IExecutionContextAccessor executionContextAccessor)
        {
            this._executionContextAccessor = executionContextAccessor;
        }

        public UserId UserId => new UserId(_executionContextAccessor.UserId);
    }
}