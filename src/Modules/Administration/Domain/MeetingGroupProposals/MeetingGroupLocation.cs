﻿using Dpk.DepositInterest.BuildingBlocks.Domain;

namespace Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals
{
    public class MeetingGroupLocation : ValueObject
    {
        private MeetingGroupLocation(string city, string countryCode)
        {
            City = city;
            CountryCode = countryCode;
        }

        public string City { get; }

        public string CountryCode { get; }

        public static MeetingGroupLocation Create(string city, string countryCode)
        {
            return new MeetingGroupLocation(city, countryCode);
        }
    }
}