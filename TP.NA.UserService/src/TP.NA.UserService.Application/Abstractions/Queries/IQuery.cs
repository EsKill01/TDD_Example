using MediatR;

namespace TP.NA.UserService.Application.Abstractions.Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}