using Autofac;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.EventBus;
using Serilog;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.EventsBus
{
    internal static class EventsBusStartup
    {
        internal static void Initialize(
            ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        private static void SubscribeToIntegrationEvents(ILogger logger)
        {
            var eventBus = AdministrationCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();

        }

        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, ILogger logger)
            where T : IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}