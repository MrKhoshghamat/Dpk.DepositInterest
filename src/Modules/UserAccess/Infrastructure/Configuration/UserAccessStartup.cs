using Autofac;
using Dpk.DepositInterest.BuildingBlocks.Application;
using Dpk.DepositInterest.BuildingBlocks.Application.Emails;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.Emails;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.EventBus;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Email;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.EventsBus;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Logging;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Mediation;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Processing;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Quartz;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration.Security;
using Serilog;

namespace Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            EmailsConfiguration emailsConfiguration,
            string textEncryptionKey,
            IEmailSender emailSender,
            IEventsBus eventsBus,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "UserAccess");

            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                logger,
                emailsConfiguration,
                textEncryptionKey,
                emailSender,
                eventsBus);

            QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            EmailsConfiguration emailsConfiguration,
            string textEncryptionKey,
            IEmailSender emailSender,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule(new BiDictionary<string, Type>()));

            containerBuilder.RegisterModule(new QuartzModule());
            containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));
            containerBuilder.RegisterModule(new SecurityModule(textEncryptionKey));

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            UserAccessCompositionRoot.SetContainer(_container);
        }
    }
}