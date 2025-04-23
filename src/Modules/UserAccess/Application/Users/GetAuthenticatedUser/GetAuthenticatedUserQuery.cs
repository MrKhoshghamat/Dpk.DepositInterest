using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Application.Users.GetUser;

namespace Dpk.DepositInterest.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    public class GetAuthenticatedUserQuery : QueryBase<UserDto>
    {
        public GetAuthenticatedUserQuery()
        {
        }
    }
}