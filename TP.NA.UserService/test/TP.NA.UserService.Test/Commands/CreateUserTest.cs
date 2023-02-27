using Microsoft.AspNetCore.Http;
using Moq;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Commands.User;
using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;
using TP.NA.UserService.Test.Factory;
using Xunit;

namespace TP.NA.UserService.Test.Commands
{
    public class CreateUserTest : AutofixData
    {
        private readonly Mock<IUserRepository> _mockIUserRepository;
        private readonly CreateUser.Handler _userHandler;
        private CancellationToken cancellationToken;

        public CreateUserTest()
        {
            _mockIUserRepository = FactoryCommon.MockIUserRepository;
            cancellationToken = FactoryCommon.CancellationToken;
            _userHandler = new CreateUser.Handler(FactoryCommon.mapperUser, _mockIUserRepository.Object);
        }

        [Fact]
        public async Task Handler_IdExist_Return400()
        {
            //Arrange
            var userRequest = UserRequestSetup();
            CreateUser.Command command = new CreateUser.Command(userRequest);
            GetUserRepositorySetup(UserSetup());

            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Null(result.Payload.UserResponse.User);
        }

        [Fact]
        public async Task Handler_IdExist_ReturnProperty()
        {
            //Arrange
            var expectedMessage = "Invalid User Id";
            var userRequest = UserRequestSetup();
            CreateUser.Command command = new CreateUser.Command(userRequest);
            GetUserRepositorySetup(UserSetup());

            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.Single(result.Errors);
            Assert.NotEmpty(result.Errors.First().Property);
            Assert.Equal(expectedMessage, result.Errors.First().Property);
        }

        [Fact]
        public async Task Handler_IdExist_ReturnMessage()
        {
            //Arrange
            var userRequest = UserRequestSetup();
            var expectedMessage = $"User {userRequest.Name} already exist";
            CreateUser.Command command = new(userRequest);
            GetUserRepositorySetup(UserSetup());

            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.Single(result.Errors);
            Assert.NotEmpty(result.Errors.First().Message);
            Assert.Equal(expectedMessage, result.Errors.First().Message);
        }

        [Fact]
        public async Task Handler_IdNotExist_CreateAndReturnUser()
        {
            //Arrange
            var userRequest = UserRequestSetup();
            CreateUser.Command command = new CreateUser.Command(userRequest);
            GetUserRepositorySetup();
            CreateUserRepositorySetup(UserSetup());

            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.NotNull(result.Payload.UserResponse.User);
            Assert.IsType<UserModel>(result.Payload.UserResponse.User);
        }

        [Fact]
        public async Task Handler_IdNotExist_ReturnOk()
        {
            //Arrange
            var userRequest = UserRequestSetup();
            CreateUser.Command command = new CreateUser.Command(userRequest);
            GetUserRepositorySetup();
            CreateUserRepositorySetup(UserSetup());

            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Handler_IdNotExist_ReturnValuesWithContent()
        {
            //Arrange
            var userRequest = UserRequestSetup();
            var userEntity = UserSetup();
            CreateUser.Command command = new CreateUser.Command(userRequest);
            GetUserRepositorySetup();
            CreateUserRepositorySetup(userEntity);
            //Act
            var result = await _userHandler.Handle(command, cancellationToken);
            //Assert
            Assert.NotEmpty(result.Payload.UserResponse.User.Id);
            Assert.NotEmpty(result.Payload.UserResponse.User.Email);
            Assert.NotEmpty(result.Payload.UserResponse.User.Password);
            Assert.NotEmpty(result.Payload.UserResponse.User.Address);
            Assert.NotEmpty(result.Payload.UserResponse.User.Country);
            Assert.NotEmpty(result.Payload.UserResponse.User.LastName);
            Assert.NotEmpty(result.Payload.UserResponse.User.PhoneNumber);
            Assert.True(result.Payload.UserResponse.User.Active);
        }

        #region Setup Repository
        private void GetUserRepositorySetup(UserEntity user = null)
        {
            _mockIUserRepository.Setup(u => u.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        }
        private void CreateUserRepositorySetup(UserEntity user)
        {
            _mockIUserRepository.Setup(u => u.AddAsync(It.IsAny<UserEntity>())).ReturnsAsync(user);
        }
        #endregion
    }
}
