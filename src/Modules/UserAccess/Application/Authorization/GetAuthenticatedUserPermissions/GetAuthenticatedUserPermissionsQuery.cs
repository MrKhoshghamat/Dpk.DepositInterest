using Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;

namespace Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions
{
    public class GetAuthenticatedUserPermissionsQuery : QueryBase<List<UserPermissionDto>>
    {
    }
}