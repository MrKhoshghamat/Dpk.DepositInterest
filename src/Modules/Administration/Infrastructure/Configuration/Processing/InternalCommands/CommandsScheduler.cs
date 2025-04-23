using Dapper;
using Dpk.DepositInterest.BuildingBlocks.Application.Data;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.InternalCommands;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.Serialization;
using Dpk.DepositInterest.Modules.Administration.Application.Configuration.Commands;
using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using Newtonsoft.Json;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IInternalCommandsMapper _internalCommandsMapper;

        public CommandsScheduler(
            ISqlConnectionFactory sqlConnectionFactory,
            IInternalCommandsMapper internalCommandsMapper)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _internalCommandsMapper = internalCommandsMapper;
        }

        public async Task EnqueueAsync(ICommand command)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [administration].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = _internalCommandsMapper.GetName(command.GetType()),
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO [administration].[InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                                     "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = _internalCommandsMapper.GetName(command.GetType()),
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });
        }
    }
}