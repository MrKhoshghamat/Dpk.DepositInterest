using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals.Rules
{
    public class MeetingGroupProposalCanBeVerifiedOnceRule : IBusinessRule
    {
        private readonly MeetingGroupProposalDecision _actualDecision;

        internal MeetingGroupProposalCanBeVerifiedOnceRule(MeetingGroupProposalDecision actualDecision)
        {
            _actualDecision = actualDecision;
        }

        public string Message => "Meeting group proposal can be verified only once";

        public bool IsBroken() => _actualDecision != MeetingGroupProposalDecision.NoDecision;
    }
}