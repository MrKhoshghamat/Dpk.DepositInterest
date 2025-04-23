using Dpk.DepositInterest.ArchTests.SeedWork;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Dpk.DepositInterest.ArchTests.Api
{
    [TestFixture]
    public class ApiTests : TestBase
    {
        [Test]
        public void AdministrationApi_DoesNotHaveDependency_ToOtherModules()
        {
            List<string> otherModules = [UserAccessNamespace];
            var result = Types.InAssembly(ApiAssembly)
                .That()
                        .ResideInNamespace("Dpk.DepositInterest.API.Modules.Administration")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void MeetingsApi_DoesNotHaveDependency_ToOtherModules()
        {
            List<string> otherModules = [AdministrationNamespace, UserAccessNamespace];
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Dpk.DepositInterest.API.Modules.Meetings")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void PaymentsApi_DoesNotHaveDependency_ToOtherModules()
        {
            List<string> otherModules = [AdministrationNamespace, UserAccessNamespace];
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Dpk.DepositInterest.API.Modules.Payments")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Test]
        public void UserAccessApi_DoesNotHaveDependency_ToOtherModules()
        {
            List<string> otherModules = [AdministrationNamespace];
            var result = Types.InAssembly(ApiAssembly)
                .That()
                .ResideInNamespace("Dpk.DepositInterest.API.Modules.UserAccess")
                .Should()
                .NotHaveDependencyOnAny(otherModules.ToArray())
                .GetResult();

            AssertArchTestResult(result);
        }
    }
}