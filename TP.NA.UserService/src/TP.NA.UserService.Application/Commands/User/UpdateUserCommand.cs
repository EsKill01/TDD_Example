using AutoMapper;
using FluentValidation;
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
    public class UpdateUserCommand
    {
        #region Result

        public class Result
        {
            public Result()
            {
                UpdateUserResponse = new UpdateUserResponse();
            }

            public UpdateUserResponse UpdateUserResponse { get; set; }
        }

        #endregion Result

        #region Command

        public class Command : CommonCommand, ICommand<Response<Result>>
        {
            public UpdateUserRequest Request { get; set; }

            public Command()
            {
                Request = new UpdateUserRequest();
            }
        }

        #endregion Command

        #region Mapper

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, UserEntity>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Request.UserId))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Request.Email))
                    .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Request.Password))
                    .ReverseMap();

                CreateMap<Command, UserModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Request.UserId))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Request.Email))
                    .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Request.Password))
                    .ReverseMap();

                CreateMap<UserEntity, UserModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Password))
                    .ReverseMap();
            }
        }

        #endregion Mapper

        #region Validation

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(c => c.Request).NotNull();
                RuleFor(c => c.Request.UserId).NotNull().NotEmpty();
                RuleFor(c => c.Request.Email).NotNull().NotEmpty();
                RuleFor(c => c.Request.Password).NotNull().NotEmpty();
            }
        }

        #endregion Validation

        #region Handler

        public class Handler : ICommandHandler<Command, Response<Result>>
        {
            private readonly IMapper _mapper;

            private readonly IUserRepository _userRepository;

            private readonly Response<Result> _response;

            public Handler(IMapper mapper, IUserRepository userRepository)
            {
                this._mapper = mapper;
                this._userRepository = userRepository;
                this._response = new Response<Result>()
                {
                    Payload = new Result()
                };
            }

            public async Task<Response<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var updateUser = await _userRepository.GetByIdAsync(request.Request.UserId);

                if (updateUser == null)
                {
                    _response.SetFailureResponse("User", "User do not exists");
                    return _response;
                }

                request.Request.UserId = updateUser.Id;

                var map = _mapper.Map<Command, UserEntity>(request);
                var result = await _userRepository.UpdateAsync(map);
                var model = _mapper.Map<UserEntity, UserModel>(result);

                _response.Payload.UpdateUserResponse.User = model;

                return _response;
            }
        }

        #endregion Handler
    }
}