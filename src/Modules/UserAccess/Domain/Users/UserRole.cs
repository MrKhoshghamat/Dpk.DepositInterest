﻿using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepostiInterest.Modules.UserAccess.Domain.Users
{
    public class UserRole : ValueObject
    {
        public static UserRole Member => new UserRole(nameof(Member));

        public static UserRole Administrator => new UserRole(nameof(Administrator));

        public string Value { get; }

        private UserRole(string value)
        {
            this.Value = value;
        }
    }
}