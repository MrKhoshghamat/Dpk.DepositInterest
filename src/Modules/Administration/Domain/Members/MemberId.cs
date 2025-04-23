using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.Members
{
    public class MemberId : TypedIdValueBase
    {
        public MemberId(Guid value)
            : base(value)
        {
        }
    }
}