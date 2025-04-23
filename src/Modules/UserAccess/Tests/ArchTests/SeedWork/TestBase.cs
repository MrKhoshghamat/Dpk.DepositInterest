using System.Reflection;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Infrastructure;
using Dpk.DepostiInterest.Modules.UserAccess.Domain.Users;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Dpk.DepositInterest.Modules.UserAccess.ArchTests.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApplicationAssembly => typeof(CommandBase).Assembly;

        protected static Assembly DomainAssembly => typeof(User).Assembly;

        protected static Assembly InfrastructureAssembly => typeof(UserAccessContext).Assembly;

        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            List<Type> failingTypes = [];
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            Assert.That(types, Is.Null.Or.Empty);
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}