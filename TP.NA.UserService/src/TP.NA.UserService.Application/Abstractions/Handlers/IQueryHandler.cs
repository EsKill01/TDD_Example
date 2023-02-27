using MediatR;
using TP.NA.UserService.Application.Abstractions.Queries;

namespace TP.NA.UserService.Application.Abstractions.Handlers
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}