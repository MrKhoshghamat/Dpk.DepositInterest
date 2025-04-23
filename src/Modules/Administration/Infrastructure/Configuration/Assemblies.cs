using System.Reflection;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IAdministrationModule).Assembly;
    }
}