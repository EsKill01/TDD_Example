using MediatR;
using TP.NA.UserService.Application.Abstractions.Commands;

namespace TP.NA.UserService.Application.Abstractions.Handlers
{
    internal interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}