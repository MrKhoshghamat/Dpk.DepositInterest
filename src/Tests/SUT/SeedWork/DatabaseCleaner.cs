using System.Data;
using Dapper;

namespace Dpk.DepositInterest.SUT.SeedWork
{
    internal static class DatabaseCleaner
    {
        internal static async Task ClearAllData(IDbConnection connection)
        {
            await ClearAdministration(connection);

            await ClearApp(connection);

            await ClearUsers(connection);
        }

        private static async Task ClearUsers(IDbConnection connection)
        {
            var clearUsersSql =
                "DELETE FROM [users].[InboxMessages] " +
                "DELETE FROM [users].[InternalCommands] " +
                "DELETE FROM [users].[OutboxMessages] " +
                "DELETE FROM [users].[Permissions] " +
                "DELETE FROM [users].[RolesToPermissions] " +
                "DELETE FROM [users].[UserRoles] " +
                "DELETE FROM [users].[Users] ";

            await connection.ExecuteScalarAsync(clearUsersSql);
        }

        private static async Task ClearApp(IDbConnection connection)
        {
            var clearAppSql =
                "DELETE FROM [app].[Emails] ";
            await connection.ExecuteScalarAsync(clearAppSql);
        }

        private static async Task ClearAdministration(IDbConnection connection)
        {
            var clearAdministrationSql =
                "DELETE FROM [administration].[InboxMessages] " +
                "DELETE FROM [administration].[InternalCommands] " +
                "DELETE FROM [administration].[MeetingGroupProposals] " +
                "DELETE FROM [administration].[Members] " +
                "DELETE FROM [administration].[OutboxMessages] ";

            await connection.ExecuteScalarAsync(clearAdministrationSql);
        }
    }
}