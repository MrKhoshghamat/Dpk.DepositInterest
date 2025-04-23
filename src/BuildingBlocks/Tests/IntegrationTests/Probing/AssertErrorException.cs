namespace Dpk.DepositInterest.BuildingBlocks.IntegrationTests.Probing
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message)
            : base(message)
        {
        }
    }
}