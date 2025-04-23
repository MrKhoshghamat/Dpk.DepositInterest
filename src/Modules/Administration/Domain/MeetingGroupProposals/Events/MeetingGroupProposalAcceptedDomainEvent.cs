using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals.Events
{
    public class MeetingGroupProposalAcceptedDomainEvent : DomainEventBase
    {
        public MeetingGroupProposalAcceptedDomainEvent(MeetingGroupProposalId meetingGroupProposalId)
        {
            MeetingGroupProposalId = meetingGroupProposalId;
        }

        public MeetingGroupProposalId MeetingGroupProposalId { get; }
    }
}