using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals.Events
{
    internal class MeetingGroupProposalRejectedDomainEvent : DomainEventBase
    {
        internal MeetingGroupProposalRejectedDomainEvent(MeetingGroupProposalId meetingGroupProposalId)
        {
            MeetingGroupProposalId = meetingGroupProposalId;
        }

        internal MeetingGroupProposalId MeetingGroupProposalId { get; }
    }
}