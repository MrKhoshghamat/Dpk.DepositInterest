﻿using MediatR;

namespace Dpk.DepositInterest.Modules.Administration.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}