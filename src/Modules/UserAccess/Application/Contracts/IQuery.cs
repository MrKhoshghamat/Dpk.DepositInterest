using MediatR;

namespace Dpk.DepositInterest.Modules.UserAccess.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}