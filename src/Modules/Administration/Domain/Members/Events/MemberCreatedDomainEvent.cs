using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.Members.Events
{
    public class MemberCreatedDomainEvent : DomainEventBase
    {
        public MemberCreatedDomainEvent(MemberId memberId)
        {
            MemberId = memberId;
        }

        public MemberId MemberId { get; }
    }
}