using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals
{
    public class MeetingGroupProposalId : TypedIdValueBase
    {
        public MeetingGroupProposalId(Guid value)
            : base(value)
        {
        }
    }
}