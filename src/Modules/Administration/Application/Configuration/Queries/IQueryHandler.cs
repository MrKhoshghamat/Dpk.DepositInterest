using Dpk.DepositInterest.Modules.Administration.Application.Contracts;
using MediatR;

namespace Dpk.DepositInterest.Modules.Administration.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}