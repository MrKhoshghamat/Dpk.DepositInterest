using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepostiInterest.Modules.UserAccess.Domain.Users
{
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}