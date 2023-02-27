using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.Configs;

namespace TP.NA.UserService.Application.EndPoints.User
{
    public class UpdateUserEndPoint : ICarterModule
    {
        public async Task<IResult> UpdateUser(IMediator mediator, [FromBody] UpdateUserCommand.Command request)
        {
            var result = await mediator.Send(request);

            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v1/userService/", UpdateUser)
                .WithDisplayName("User")
                .Accepts<UpdateUserCommand.Command>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces<IResponse>(StatusCodes.Status400BadRequest)
                .Produces<IResponse>(StatusCodes.Status422UnprocessableEntity)
                .AddEndpointFilter<ValidationFilter<UpdateUserCommand.Command, UpdateUserCommand.Result>>(); ;
        }
    }
}