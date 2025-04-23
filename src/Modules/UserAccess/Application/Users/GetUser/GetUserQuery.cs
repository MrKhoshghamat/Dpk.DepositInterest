using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;

namespace Dpk.DepositInterest.Modules.UserAccess.Application.Users.GetUser
{
    public class GetUserQuery : QueryBase<UserDto>
    {
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}