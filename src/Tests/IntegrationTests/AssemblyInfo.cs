using NUnit.Framework;

[assembly: NonParallelizable]
[assembly: LevelOfParallelism(1)]

namespace Dpk.DepositInterest.IntegrationTests
{
    public class AssemblyInfo
    {
    }
}