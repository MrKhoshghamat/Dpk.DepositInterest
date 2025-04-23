using NUnit.Framework;

[assembly: NonParallelizable]
[assembly: LevelOfParallelism(1)]

namespace Dpk.DepositInterest.Modules.UserAccess.IntegrationTests
{
    public class AssemblyInfo
    {
    }
}