using System.Reflection;
using Dpk.DepositInterest.Modules.Administration.Application.Members.CreateMember;
using Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals;
using Dpk.DepositInterest.Modules.Administration.Infrastructure;
using NetArchTest.Rules;
using NUnit.Framework;

namespace Dpk.DepositInterest.Modules.Administration.ArchTests.SeedWork
{
    public abstract class TestBase
    {
        protected static Assembly ApplicationAssembly => typeof(CreateMemberCommand).Assembly;

        protected static Assembly DomainAssembly => typeof(MeetingGroupProposal).Assembly;

        protected static Assembly InfrastructureAssembly => typeof(AdministrationContext).Assembly;

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