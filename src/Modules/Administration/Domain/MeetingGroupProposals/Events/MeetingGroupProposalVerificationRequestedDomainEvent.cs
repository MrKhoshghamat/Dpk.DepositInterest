using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals.Events
{
    public class MeetingGroupProposalVerificationRequestedDomainEvent : DomainEventBase
    {
        internal MeetingGroupProposalVerificationRequestedDomainEvent(MeetingGroupProposalId meetingGroupProposalId)
        {
            MeetingGroupProposalId = meetingGroupProposalId;
        }

        public MeetingGroupProposalId MeetingGroupProposalId { get; }
    }
}