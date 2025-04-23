using Dpk.DepositInterest.API.Configuration.Authorization;
using Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions;
using Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Dpk.DepositInterest.Modules.UserAccess.Application.Contracts;
using Dpk.DepositInterest.Modules.UserAccess.Application.Users.GetAuthenticatedUser;
using Dpk.DepositInterest.Modules.UserAccess.Application.Users.GetUser;
using Microsoft.AspNetCore.Mvc;

namespace Dpk.DepositInterest.API.Modules.UserAccess
{
    [Route("api/userAccess/authenticatedUser")]
    [ApiController]
    public class AuthenticatedUserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public AuthenticatedUserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [NoPermissionRequired]
        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserQuery());

            return Ok(user);
        }

        [NoPermissionRequired]
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(List<UserPermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUserPermissions()
        {
            var permissions = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserPermissionsQuery());

            return Ok(permissions);
        }
    }
}