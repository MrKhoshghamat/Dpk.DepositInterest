﻿namespace Dpk.DepositInterest.SUT.SeedWork.Probing
{
    public class AssertErrorException : Exception
    {
        public AssertErrorException(string message)
            : base(message)
        {
        }
    }
}