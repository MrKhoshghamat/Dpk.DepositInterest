using NUnit.Framework;

[assembly: NonParallelizable]
[assembly: LevelOfParallelism(1)]

namespace Dpk.DepositInterest.Modules.Administration.IntegrationTests
{
    public class AssemblyInfo
    {
    }
}