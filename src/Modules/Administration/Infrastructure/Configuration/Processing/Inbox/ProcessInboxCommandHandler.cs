﻿using Dapper;
using Dpk.DepositInterest.BuildingBlocks.Application.Data;
using Dpk.DepositInterest.Modules.Administration.Application.Configuration.Commands;
using MediatR;
using Newtonsoft.Json;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure.Configuration.Processing.Inbox
{
    internal class ProcessInboxCommandHandler : ICommandHandler<ProcessInboxCommand>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Handle(ProcessInboxCommand command, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = $"""
                               SELECT 
                                   [InboxMessage].[Id] AS [{nameof(InboxMessageDto.Id)}], 
                                   [InboxMessage].[Type] AS [{nameof(InboxMessageDto.Type)}], 
                                   [InboxMessage].[Data] AS [{nameof(InboxMessageDto.Data)}] 
                               FROM [administration].[InboxMessages] AS [InboxMessage] 
                               WHERE [InboxMessage].[ProcessedDate] IS NULL 
                               ORDER BY [InboxMessage].[OccurredOn]
                               """;

            var messages = await connection.QueryAsync<InboxMessageDto>(sql);

            const string sqlUpdateProcessedDate = """
                                                  UPDATE [administration].[InboxMessages] 
                                                  SET [ProcessedDate] = @Date 
                                                  WHERE [Id] = @Id
                                                  """;

            foreach (var message in messages)
            {
                var messageAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(assembly => message.Type.Contains(assembly.GetName().Name));

                Type type = messageAssembly.GetType(message.Type);
                var request = JsonConvert.DeserializeObject(message.Data, type);

                try
                {
                    await _mediator.Publish((INotification)request, cancellationToken);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                await connection.ExecuteScalarAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });
            }
        }
    }
}