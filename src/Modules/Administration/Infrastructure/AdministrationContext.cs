using Dpk.DepositInterest.BuildingBlocks.Application.Outbox;
using Dpk.DepositInterest.BuildingBlocks.Infrastructure.InternalCommands;
using Dpk.DepositInterest.Modules.Administration.Domain.MeetingGroupProposals;
using Dpk.DepositInterest.Modules.Administration.Domain.Members;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Domain.MeetingGroupProposals;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Domain.Members;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.InternalCommands;
using Dpk.DepositInterest.Modules.Administration.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dpk.DepositInterest.Modules.Administration.Infrastructure
{
    public class AdministrationContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<InternalCommand> InternalCommands { get; set; }

        internal DbSet<MeetingGroupProposal> MeetingGroupProposals { get; set; }

        internal DbSet<OutboxMessage> OutboxMessages { get; set; }

        internal DbSet<Member> Members { get; set; }

        public AdministrationContext(DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MeetingGroupProposalEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
        }
    }
}