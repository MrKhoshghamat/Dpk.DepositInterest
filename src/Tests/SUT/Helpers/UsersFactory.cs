using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Application.Users.AddAdminUser;
using Dpk.DepositInterest.SUT.SeedWork;

namespace Dpk.DepositInterest.SUT.Helpers
{
    internal static class UsersFactory
    {
        public static async Task GivenAdmin(
            IUserAccessModule userAccessModule,
            string login,
            string password,
            string name,
            string firstName,
            string lastName,
            string email)
        {
            await userAccessModule.ExecuteCommandAsync(new AddAdminUserCommand(
                login,
                password,
                firstName,
                lastName,
                name,
                email));
        }
    }
}