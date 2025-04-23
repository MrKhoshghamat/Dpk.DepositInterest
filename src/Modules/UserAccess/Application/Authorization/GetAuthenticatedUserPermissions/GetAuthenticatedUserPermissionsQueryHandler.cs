using Dapper;
using Dpk.DepositInterest.BuildingBlocks.Application;
using Dpk.DepositInterest.BuildingBlocks.Application.Data;
using Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Dpk.DepositInterest.Modules.UserAccess.Application.Configuration.Queries;

namespace Dpk.DepositInterest.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions
{
    internal class GetAuthenticatedUserPermissionsQueryHandler : IQueryHandler<GetAuthenticatedUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserPermissionsQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IExecutionContextAccessor executionContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<List<UserPermissionDto>> Handle(GetAuthenticatedUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (!_executionContextAccessor.IsAvailable)
            {
                return [];
            }

            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = $""" 
                               SELECT [UserPermission].[PermissionCode] AS [{nameof(UserPermissionDto.Code)}]
                               FROM [users].[v_UserPermissions] AS [UserPermission] 
                               WHERE [UserPermission].UserId = @UserId
                               """;

            var permissions = await connection.QueryAsync<UserPermissionDto>(
                sql,
                new { _executionContextAccessor.UserId });

            return permissions.AsList();
        }
    }
}