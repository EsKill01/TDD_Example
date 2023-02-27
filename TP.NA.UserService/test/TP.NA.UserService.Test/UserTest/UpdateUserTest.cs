using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Net;
using System.Net.Http.Json;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Commands.User.Response;
using TP.NA.UserService.Application.Commons;
using TP.NA.UserService.Application.EndPoints.User;
using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;
using TP.NA.UserService.Test.MapperConfig;
using Xunit;
using static TP.NA.UserService.Application.Commands.User.UpdateUserCommand;

namespace TP.NA.UserService.Test.UserTest
{
    public class UpdateUserTest
    {
        private Mock<IUserRepository> userRepository;
        private AutoMapper.Mapper mapper;
        private Mock<IMediator> mediatorMock;

        public UpdateUserTest()
        {
            userRepository = new Mock<IUserRepository>();
            mapper = new AutoMapper.Mapper(AutoMapperConf.Config());
            mediatorMock = new Mock<IMediator>();
        }

        [Theory]
        [InlineData("1234")]
        public async Task GetIfUserExists(string id)
        {
            //arrange
            var entityResponse = UserMockData.GetUserEntities.FirstOrDefault(x => x.Id == id);
            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(entityResponse);

            //act

            var result = await userRepository.Object.GetByIdAsync(id);

            //assert

            Assert.NotNull(result);
            Assert.IsType<UserEntity>(result);

            reset();
        }

        [Theory]
        [InlineData("1234")]
        public async Task ValidateIfIdIsTheSameAsEnterIdParameter(string id)
        {
            //arrange
            var entityResponse = UserMockData.GetUserEntities.FirstOrDefault(x => x.Id == id);
            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(entityResponse);
            //act

            var result = await userRepository.Object.GetByIdAsync(id);

            //assert

            Assert.True(result.Id == id);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateAllObjectAllowValues(string userId, string email, string password)
        {
            //arrange

            var updateUser = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            userRepository.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(updateUser);

            //act

            var result = await userRepository.Object.UpdateAsync(updateUser);

            //assert

            Assert.NotNull(result);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserHandlerCallNotNull(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var handler = new UpdateUserCommand.Handler(mapper, userRepository.Object);
            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(model);
            userRepository.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(model);

            //act

            var handlerResult = await handler.Handle(command, default);

            //assert

            Assert.NotNull(handlerResult);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserHandlerCallUsingRepository(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var handler = new UpdateUserCommand.Handler(mapper, userRepository.Object);

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            userRepository.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(model);
            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(model);

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var handlerResult = await handler.Handle(command, default);

            //assert

            Assert.NotNull(handlerResult);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserHandlerValidateResponse(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var handler = new UpdateUserCommand.Handler(mapper, userRepository.Object);

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            userRepository.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(model);
            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(model);

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var handlerResult = await handler.Handle(command, default);

            //assert

            Assert.NotNull(handlerResult);
            Assert.NotNull(handlerResult.Payload);
            Assert.NotNull(handlerResult.Payload.UpdateUserResponse);
            Assert.NotNull(handlerResult.Payload.UpdateUserResponse.User);
            Assert.IsType<UpdateUserCommand.Result>(handlerResult.Payload);
            Assert.IsType<UpdateUserResponse>(handlerResult.Payload.UpdateUserResponse);
            Assert.IsType<UserModel>(handlerResult.Payload.UpdateUserResponse.User);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserHandlerValidateHttpOkResponse(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var handler = new UpdateUserCommand.Handler(mapper, userRepository.Object);

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            userRepository.Setup(m => m.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(model);
            userRepository.Setup(m => m.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(model);

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var handlerResult = await handler.Handle(command, default);

            //assert

            Assert.True(handlerResult.StatusCode.Equals(StatusCodes.Status200OK));

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserDelegateResponseNotNull(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var endPointDelegate = new UpdateUserEndPoint();

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            var response = new Response<UpdateUserCommand.Result>();

            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var delegateResult = await endPointDelegate.UpdateUser(mediatorMock.Object, command);

            //assert

            Assert.NotNull(delegateResult);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserDelegateResponseWithUserEntity(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var endPointDelegate = new UpdateUserEndPoint();

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            var response = new Response<UpdateUserCommand.Result>();

            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var delegateResult = await endPointDelegate.UpdateUser(mediatorMock.Object, command);

            //assert

            Assert.IsType<Ok<Response<Result>>>(delegateResult);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserDelegateMediatorCall(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var endPointDelegate = new UpdateUserEndPoint();

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            var response = new Response<UpdateUserCommand.Result>();

            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var delegateResult = await endPointDelegate.UpdateUser(mediatorMock.Object, command);

            //assert

            mediatorMock.Verify(x => x.Send(It.IsAny<UpdateUserCommand.Command>(), default), Times.Once);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserDelegateMediatorCallResponseObject(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var endPointDelegate = new UpdateUserEndPoint();

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            var response = new Response<UpdateUserCommand.Result>();
            response.Payload = new UpdateUserCommand.Result();
            response.Payload.UpdateUserResponse.User = mapper.Map<UserModel>(model);

            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var delegateResult = await endPointDelegate.UpdateUser(mediatorMock.Object, command);

            //assert

            mediatorMock.Verify(x => x.Send(It.IsAny<UpdateUserCommand.Command>(), default), Times.Once);

            var objResult = ((Ok<Response<UpdateUserCommand.Result>>)delegateResult).Value;

            Assert.NotNull(objResult);
            Assert.IsType<Response<UpdateUserCommand.Result>>(objResult);
            Assert.NotNull(objResult.Payload);
            Assert.IsType<UpdateUserCommand.Result>(objResult.Payload);
            Assert.NotNull(objResult.Payload.UpdateUserResponse);
            Assert.IsType<UpdateUserResponse>(objResult.Payload.UpdateUserResponse);
            Assert.NotNull(objResult.Payload.UpdateUserResponse.User);
            Assert.IsType<UserModel>(objResult.Payload.UpdateUserResponse.User);

            reset();
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserDelegateMediatorCallResponseBadRequest(string userId, string email, string password)
        {
            //assert

            var command = new UpdateUserCommand.Command();
            var endPointDelegate = new UpdateUserEndPoint();

            var model = new UserEntity
            {
                Id = userId,
                Email = email,
                Password = password
            };

            var response = new Response<UpdateUserCommand.Result>();
            response.IsError = true;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Errors = new List<ValidationMessage>
            {
                new ValidationMessage
                {
                    Message = "Error",
                    Property = "Error"
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<UpdateUserCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            //act

            var delegateResult = await endPointDelegate.UpdateUser(mediatorMock.Object, command);

            //assert

            mediatorMock.Verify(x => x.Send(It.IsAny<UpdateUserCommand.Command>(), default), Times.Once);

            var objResult = ((BadRequest<Response<UpdateUserCommand.Result>>)delegateResult).Value;

            Assert.NotNull(objResult);
            Assert.Null(objResult.Payload);
            Assert.True(response.IsError);
            Assert.True(response.StatusCode == (int)HttpStatusCode.BadRequest);
            Assert.True(response.Errors.Any());

            reset();
        }

        #region Integration test

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserCallWebApiNotFoundResult(string userId, string email, string password)
        {
            //arrange

            var command = new UpdateUserCommand.Command();
            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            var entity = new UserEntity { Id = userId, Email = email, Password = password };

            var url = $"/api/v1/userService/";
            userRepository.Setup(n => n.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(entity);

            await using var application = new TestHostClass(userRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PutAsJsonAsync(url, command);

            //assert

            Assert.Equal(HttpStatusCode.BadRequest, statusResponse.StatusCode);
        }

        [Theory]
        [InlineData("1234", "new_email", "new_password")]
        public async Task UpdateUserCallWebApiOkResult(string userId, string email, string password)
        {
            //arrange

            var command = new UpdateUserCommand.Command();
            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            var entity = new UserEntity { Id = userId, Email = email, Password = password };

            var url = $"/api/v1/userService/";
            userRepository.Setup(n => n.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(entity);
            userRepository.Setup(n => n.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(entity);

            await using var application = new TestHostClass(userRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PutAsJsonAsync(url, command);

            //assert

            Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
        }

        [Theory]
        [InlineData("", "new_email", "new_password")]
        public async Task UpdateUserCallWebApiValidateObjectUnsingFluentValidation(string userId, string email, string password)
        {
            //arrange

            var command = new UpdateUserCommand.Command();
            command.Request.Email = email;
            command.Request.Password = password;
            command.Request.UserId = userId;

            var entity = new UserEntity { Id = userId, Email = email, Password = password };

            var url = $"/api/v1/userService/";
            userRepository.Setup(n => n.UpdateAsync(It.IsAny<UserEntity>())).ReturnsAsync(entity);
            userRepository.Setup(n => n.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(entity);

            await using var application = new TestHostClass(userRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PutAsJsonAsync(url, command);

            //assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, statusResponse.StatusCode);

            reset();
        }

        #endregion Integration test

        private void reset()
        {
            mediatorMock.Reset();
            userRepository.Reset();
        }
    }
}