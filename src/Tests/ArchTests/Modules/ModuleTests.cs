using System.Reflection;
using Dpk.DepositInterest.ArchTests.SeedWork;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals;
using Dpk.DepositInterest.Modules.Administration.Infrastructure;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure;
using Dpk.DepostiInterest.Modules.UserAccess.Domain.Users;
using MediatR;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Dpk.DepositInterest.ArchTests.Modules
{
    [TestFixture]
    public class ModuleTests : TestBase
    {
        [Test]
        public void AdministrationModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            List<string> otherModules = [UserAccessNamespace];
            List<Assembly> administrationAssemblies =
            [
                typeof(IAdministrationModule).Assembly,
                typeof(MeetingGroupLocation).Assembly,
                typeof(AdministrationContext).Assembly
            ];

            var result = Types.InAssemblies(administrationAssemblies)
                .That()
                    .DoNotImplementInterface(typeof(INotificationHandler<>))
                    .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
                    .And().DoNotHaveName("EventsBusStartup")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void UserAccessModule_DoesNotHave_Dependency_On_Other_Modules()
        {
            List<string> otherModules = [AdministrationNamespace];
            List<Assembly> userAccessAssemblies =
            [
                typeof(IUserAccessModule).Assembly,
                typeof(User).Assembly,
                typeof(UserAccessContext).Assembly
            ];

            var result = Types.InAssemblies(userAccessAssemblies)
                .That()
                .DoNotImplementInterface(typeof(INotificationHandler<>))
                .And().DoNotHaveNameEndingWith("IntegrationEventHandler")
                .And().DoNotHaveName("EventsBusStartup")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}