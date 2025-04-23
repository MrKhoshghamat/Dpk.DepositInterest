using Autofac;
using Dpk.DepositInterest.BuildingBlocks.Application;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.EventBus;
using Dpk.DepositInterest.Modules.Administration.Application.Members.CreateMember;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Authentication;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.DataAccess;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.EventsBus;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Logging;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Mediation;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing.InternalCommands;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing.Outbox;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Quartz;
using Serilog;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration
{
    public class AdministrationStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "Administration");

            ConfigureContainer(connectionString, executionContextAccessor, moduleLogger, eventsBus);

            QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        public static void Stop()
        {
            QuartzStartup.StopQuartz();
        }

        private static void ConfigureContainer(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));

            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new AuthenticationModule());

            var domainNotificationsMap = new BiDictionary<string, Type>();
            containerBuilder.RegisterModule(new OutboxModule(domainNotificationsMap));

            BiDictionary<string, Type> internalCommandsMap = new BiDictionary<string, Type>();
            internalCommandsMap.Add("CreateMember", typeof(CreateMemberCommand));
            containerBuilder.RegisterModule(new InternalCommandsModule(internalCommandsMap));

            containerBuilder.RegisterModule(new QuartzModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            AdministrationCompositionRoot.SetContainer(_container);
        }
    }
}