﻿using Dapper;
using Dpk.DepositInterest.BuildingBlocks.Application.Data;
using Dpk.DepositInterest.BuildingBlocks.Application.Emails;
using Serilog;

namespace Dpk.DepositInterest.BuildingBlocks.Infrastructure.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        private readonly EmailsConfiguration _configuration;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public EmailSender(
            ILogger logger,
            EmailsConfiguration configuration,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task SendEmail(EmailMessage message)
        {
            var sqlConnection = _sqlConnectionFactory.GetOpenConnection();

            await sqlConnection.ExecuteScalarAsync(
                "INSERT INTO [app].[Emails] ([Id], [From], [To], [Subject], [Content], [Date]) " +
                "VALUES (@Id, @From, @To, @Subject, @Content, @Date) ",
                new
                {
                    Id = Guid.NewGuid(),
                    From = _configuration.FromEmail,
                    message.To,
                    message.Subject,
                    message.Content,
                    Date = DateTime.UtcNow
                });

            _logger.Information(
                "Email sent. From: {From}, To: {To}, Subject: {Subject}, Content: {Content}.",
                _configuration.FromEmail,
                message.To,
                message.Subject,
                message.Content);
        }
    }
}