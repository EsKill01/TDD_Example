using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using TP.NA.UserService.Application.Abstractions.Commands;
using TP.NA.UserService.Application.Abstractions.Handlers;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Commands.User.Request;
using TP.NA.UserService.Application.Commands.User.Response;
using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Application.Commands.User
{
    public class CreateUser
    {
        #region Result
        public class Result
        {
            public CreateUserResponse UserResponse { get; set; }

            public Result()
            {
                UserResponse = new CreateUserResponse();
            }
        }
        #endregion
        #region Command
        public class Command : ICommand<Response<Result>>
        {
            public CreateUserRequest UserRequest { get; set; }

            public Command(CreateUserRequest userRequest)
            {
                UserRequest = userRequest;
            }
        }
        #endregion
        #region Handler
        public class Handler : ICommandHandler<Command, Response<Result>>
        {
            private readonly IMapper _mapper;
            private readonly IUserRepository _userRepository;
            private readonly Response<Result> _response;

            public Handler(IMapper mapper, IUserRepository userRepository)
            {
                _mapper = mapper;
                _userRepository = userRepository;
                _response = new Response<Result>
                {
                    Payload = new Result()
                };
            }

            public async Task<Response<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _userRepository.GetByNameAsync(request.UserRequest.Name);
                    if (user != null)
                    {
                        _response.SetFailureResponse("Invalid User Id", $"User {request.UserRequest.Name} already exist");
                        _response.Payload.UserResponse.StatusCode = StatusCodes.Status400BadRequest;
                        return _response;
                    }
                    var map = _mapper.Map<CreateUserRequest, UserEntity>(request.UserRequest);
                    var createdUser = await _userRepository.AddAsync(map);
                    _response.Payload.UserResponse.User = _mapper.Map<UserEntity, UserModel>(createdUser);
                }
                catch (Exception ex)
                {
                    _response.SetFailureResponse("Invalid User Id", ex.Message);
                    _response.Payload.UserResponse.StatusCode = StatusCodes.Status500InternalServerError;
                }
                return _response;
            }
        }
        #endregion

        #region Validations
        public class ValidationCommandCreateUser : AbstractValidator<CreateUserRequest> {
            public ValidationCommandCreateUser()
            {
                RuleFor(s => s.Email).NotNull().NotEmpty();
                RuleFor(s => s.Address).NotNull().NotEmpty();
                RuleFor(s => s.PhoneNumber).NotNull().NotEmpty();
                RuleFor(s => s.Country).NotNull().NotEmpty();
                RuleFor(s => s.LastName).NotNull().NotEmpty();
                RuleFor(s => s.Name).NotNull().NotEmpty();
                RuleFor(s => s.Password).NotNull().NotEmpty();
            }
        }
        #endregion
    }
}
