using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public interface IDomainEventsAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}