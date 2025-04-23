using Dpk.DepositInterest.BuildingBlocks.Infrastructure.EventBus;

namespace Dpk.DepositInterest.Modules.Administration.IntegrationEvents.MeetingGroupProposals
{
    public class MeetingGroupProposalAcceptedIntegrationEvent : IntegrationEvent
    {
        public MeetingGroupProposalAcceptedIntegrationEvent(
            Guid id,
            DateTime occurredOn,
            Guid meetingGroupProposalId)
            : base(id, occurredOn)
        {
            MeetingGroupProposalId = meetingGroupProposalId;
        }

        public Guid MeetingGroupProposalId { get; }
    }
}
