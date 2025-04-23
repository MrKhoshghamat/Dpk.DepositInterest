using System.Reflection;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;

namespace Dpk.DepositInterest.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IUserAccessModule).Assembly;
    }
}