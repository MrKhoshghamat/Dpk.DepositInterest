using System.Data.SqlClient;
using Dapper;
using Dpk.DepositInterest.BuildingBlocks.Application.Emails;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.Emails;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.EventBus;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Dpk.DepositInterest.Modules.Administration.Infrastructure;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration;
using Dpk.DepositInterest.SUT.SeedWork.Probing;
using NSubstitute;
using NUnit.Framework;
using Polly.Utilities;
using Serilog;

namespace Dpk.DepositInterest.SUT.SeedWork
{
    public class TestBase
    {
        protected string ConnectionString { get; set; }

        protected virtual bool PerformDatabaseCleanup => false;

        protected virtual bool CreatePermissions => true;

        protected IEmailSender EmailSender { get; private set; }

        protected ILogger Logger { get; private set; }

        protected IUserAccessModule UserAccessModule { get; private set; }

        protected IAdministrationModule AdministrationModule { get; private set; }

        protected ExecutionContextMock ExecutionContextAccessor { get; private set; }

        protected IEventsBus EventsBus { get; private set; }

        [SetUp]
        public async Task BeforeEachTest()
        {
            SetConnectionString();

            if (PerformDatabaseCleanup)
            {
                await this.ClearDatabase();
            }

            if (CreatePermissions)
            {
                await this.SeedPermissions();
            }

            ExecutionContextAccessor = new ExecutionContextMock(Guid.NewGuid());

            var emailsConfiguration = new EmailsConfiguration("from@email.com");

            Logger = Substitute.For<ILogger>();

            EventsBus = new InMemoryEventBusClient(Logger);

            InitializeUserAccessModule(emailsConfiguration);

            InitializeAdministrationModule();

        }

        public static async Task<T> GetEventually<T>(IProbe<T> probe, int timeout)
            where T : class
        {
            var poller = new Poller(timeout);

            return await poller.GetAsync(probe);
        }

        [TearDown]
        public void AfterEachTest()
        {
            SystemClock.Reset();
        }

        protected async Task WaitForAsyncOperations()
        {
            await AsyncOperationsHelper.WaitForProcessing(ConnectionString);
        }

        protected async Task ExecuteScript(string scriptPath)
        {
            var sql = await File.ReadAllTextAsync(scriptPath);

            await using var sqlConnection = new SqlConnection(ConnectionString);
            await sqlConnection.ExecuteScalarAsync(sql);
        }

        private void InitializeAdministrationModule()
        {
            AdministrationStartup.Initialize(
                ConnectionString,
                ExecutionContextAccessor,
                Logger,
                EventsBus,
                100);

            AdministrationModule = new AdministrationModule();
        }

        private async Task SeedPermissions()
        {
            await ExecuteScript("Scripts/SeedPermissions.sql");
        }

        private void InitializeUserAccessModule(EmailsConfiguration emailsConfiguration)
        {
            Logger = Substitute.For<ILogger>();
            EmailSender = Substitute.For<IEmailSender>();

            UserAccessStartup.Initialize(
                ConnectionString,
                ExecutionContextAccessor,
                Logger,
                emailsConfiguration,
                "key",
                EmailSender,
                EventsBus,
                100);

            UserAccessModule = new UserAccessModule();
        }

        private void SetConnectionString()
        {
            const string connectionStringEnvironmentVariable = "DepositInterest_SUTDatabaseConnectionString";
            ConnectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to SUT database using environment variable: {connectionStringEnvironmentVariable}");
            }
        }

        private async Task ClearDatabase()
        {
            await using var sqlConnection = new SqlConnection(ConnectionString);
            await DatabaseCleaner.ClearAllData(sqlConnection);
        }
    }
}