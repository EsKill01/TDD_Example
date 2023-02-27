using MediatR;

namespace TP.NA.UserService.Application.Abstractions.Commands
{
    public interface ICommand<out IResponse> : IRequest<IResponse>
    {
    }
}