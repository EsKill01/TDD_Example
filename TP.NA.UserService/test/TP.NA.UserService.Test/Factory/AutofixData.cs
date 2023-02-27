using AutoFixture;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Commands.User.Request;
using TP.NA.UserService.Application.Commands.User.Response;
using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Test.Factory
{
    public class AutofixData
    {
        public UserEntity UserSetup() => new Fixture().Create<UserEntity>();
        public UserModel UserModelSetup() => new Fixture().Create<UserModel>();
        public CreateUserRequest UserRequestSetup() => new Fixture().Create<CreateUserRequest>();
        public CreateUserResponse UserResponseSetup() => new Fixture().Create<CreateUserResponse>();
        public List<ValidationMessage> ValidationMessageSetup(int numberOfMessage) => new Fixture().CreateMany<ValidationMessage>(numberOfMessage).ToList();
        public CreateUser.Command UserCommandSetup() => new Fixture().Create<CreateUser.Command>();
    }
}
