using MediatR;
using Moq;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Test.Factory;
using TP.NA.UserService.Application.Commons;
using static TP.NA.UserService.Application.Commands.User.CreateUser;
using TP.NA.UserService.Application.EndPoints.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;

namespace TP.NA.UserService.Test.Endpoints
{
    public class CreateUserEndpointTest : AutofixData
    {
        private readonly Mock<IMediator> _mediator;
        private readonly CancellationToken cancellationToken;
        private CreateUserEndpoint createUserEndpoint;
        public CreateUserEndpointTest()
        {
            cancellationToken = FactoryCommon.CancellationToken;
            _mediator = FactoryCommon.MockMediator;
            createUserEndpoint = new CreateUserEndpoint();
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnPayloadWithData()
        {
            //Arrange
            Response<Result> response = new Response<Result>()
            {
                StatusCode = 200,
                Errors = null,
                IsError = false,
                Payload = new Result { UserResponse = UserResponseSetup() }
            };
            SendSetup(response);
            //Act
            var result = await createUserEndpoint.CreateUser(_mediator.Object, UserRequestSetup());
            var content = (Ok<Response<Result>>)result;
            
            //Assert
            Assert.NotNull(content.Value.Payload.UserResponse.User);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnPayloadWithDataNoEmpty()
        {
            //Arrange
            Response<Result> response = new Response<Result>()
            {
                StatusCode = 200,
                Errors = null,
                IsError = false,
                Payload = new Result { UserResponse = UserResponseSetup() }
            };
            SendSetup(response);
            //Act
            var result = await createUserEndpoint.CreateUser(_mediator.Object, UserRequestSetup());
            var content = (Ok<Response<Result>>)result;

            //Assert
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.Id);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.LastName);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.Address);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.Country);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.Name);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.PhoneNumber);
            Assert.NotEmpty(content.Value.Payload.UserResponse.User.Email);
            Assert.True(content.Value.Payload.UserResponse.User.Active);
        }

        [Fact]
        public async Task CreateUser_InvalidUser_ReturnBadRequest()
        {
            //Arrange
            Response<Result> response = new Response<Result>()
            {
                StatusCode = 400,
                Errors = ValidationMessageSetup(1),
                IsError = true,
                Payload = new Result {}
            };
            SendSetup(response);
            //Act
            var result = await createUserEndpoint.CreateUser(_mediator.Object, UserRequestSetup());
            var content = (BadRequest<Response<Result>>)result;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, content.StatusCode);
        }

        [Fact]
        public async Task CreateUser_InvalidUser_ReturnBadRequestAndErrors()
        {
            //Arrange
            Response<Result> response = new Response<Result>()
            {
                StatusCode = 400,
                Errors = ValidationMessageSetup(1),
                IsError = true,
                Payload = new Result {  }
            };
            SendSetup(response);
            //Act
            var result = await createUserEndpoint.CreateUser(_mediator.Object, UserRequestSetup());
            var content = (BadRequest<Response<Result>>)result;

            //Assert
            Assert.True(content.Value.IsError);
            Assert.Single(content.Value.Errors);
        }

        [Fact]
        public async Task CreateUser_InvalidUser_ReturnBadRequestAndErrorsProperty()
        {
            //Arrange
            Response<Result> response = new Response<Result>()
            {
                StatusCode = 400,
                Errors = ValidationMessageSetup(1),
                IsError = true,
                Payload = new Result {  }
            };
            SendSetup(response);
            //Act
            var result = await createUserEndpoint.CreateUser(_mediator.Object, UserRequestSetup());
            var content = (BadRequest<Response<Result>>)result;

            //Assert
            Assert.NotEmpty(content.Value.Errors.First().Property);
            Assert.IsType<string>(content.Value.Errors.First().Property);
            Assert.NotEmpty(content.Value.Errors.First().Message);
            Assert.IsType<string>(content.Value.Errors.First().Message);
        }

        #region Setup Mediator
        private void SendSetup(Response<Result> response)
        {
            _mediator.Setup(s => s.Send(It.IsAny<CreateUser.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }
        #endregion
    }
}
