using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Commands.User.Request;
using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.Configs;

namespace TP.NA.UserService.Application.EndPoints.User
{
    public class CreateUserEndpoint : ICarterModule
    {
        public async Task<IResult> CreateUser(IMediator mediator, [FromBody] CreateUserRequest user)
        {
            var command = new CreateUser.Command(user);
            var result = await mediator.Send(command);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        }
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/CreateUser", CreateUser)
                .WithDisplayName("User")
                .Accepts<CreateUserRequest>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces<IResponse>(StatusCodes.Status400BadRequest)
                .Produces<IResponse>(StatusCodes.Status422UnprocessableEntity)
                .AddEndpointFilter<ValidationFilter<CreateUserRequest, CreateUser.Result>>();
        }
    }
}
