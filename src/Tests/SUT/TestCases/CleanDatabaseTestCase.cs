using Dpk.DepositInterest.SUT.SeedWork;
using NUnit.Framework;

namespace Dpk.DepositInterest.SUT.TestCases
{
    public class CleanDatabaseTestCase : TestBase
    {
        protected override bool PerformDatabaseCleanup => true;

        protected override bool CreatePermissions => false;

        [Test]
        public void Prepare()
        {
        }
    }
}