using MediatR;

namespace Dpk.DepositInterest.BuildingBlocks.Application.Events
{
    public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotification : INotification
    {
        Guid Id { get; }
    }
}