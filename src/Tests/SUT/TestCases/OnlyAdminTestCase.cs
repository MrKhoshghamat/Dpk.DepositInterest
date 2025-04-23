using Dpk.DepositInterest.SUT.Helpers;
using Dpk.DepositInterest.SUT.SeedWork;
using NUnit.Framework;

namespace Dpk.DepositInterest.SUT.TestCases
{
    public class OnlyAdminTestCase : TestBase
    {
        protected override bool PerformDatabaseCleanup => true;

        [Test]
        public async Task Prepare()
        {
            await UsersFactory.GivenAdmin(
                UserAccessModule,
                "testAdmin@mail.com",
                "testAdminPass",
                "Jane Doe",
                "Jane",
                "Doe",
                "testAdmin@mail.com");
        }
    }
}