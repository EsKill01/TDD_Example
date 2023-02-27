using Moq;
using System.Net;
using System.Net.Http.Json;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Domain.Entities;
using TP.NA.UserService.Test.Factory;
using Xunit;

namespace TP.NA.UserService.Test.Routes.User
{
    public class CreateUserRouteTest : AutofixData
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly string urlCreateUser;
        public CreateUserRouteTest()
        {
            _mockUserRepository = FactoryCommon.MockIUserRepository;
            urlCreateUser = $"/api/v1/CreateUser";
        }

        [Fact]
        public async Task CreateUserRoute_ValidParameter_ReturnSuccess()
        {
            //Arrange
            var user = UserSetup();
            var createUserRequest = UserRequestSetup();
            CreateUserSetup(user);
            await using var application = new TestHostClass(_mockUserRepository.Object);
            var client = application.CreateClient();
            //Act
            var statusResponse = await client.PostAsJsonAsync(urlCreateUser, createUserRequest);
            //Assert
            Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
        }

        [Fact]
        public async Task CreateUserRoute_ValidParameter_ValidateProperties()
        {
            //Arrange
            var user = UserSetup();
            var createUserRequest = UserRequestSetup();
            CreateUserSetup(user);
            await using var application = new TestHostClass(_mockUserRepository.Object);
            var client = application.CreateClient();
            //Act
            var result = await client.PostAsJsonAsync(urlCreateUser, createUserRequest);
            //Assert
            Assert.Contains(user.Id, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.Name, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.LastName, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.Country, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.Address, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.PhoneNumber, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.Email, await result.Content.ReadAsStringAsync());
            Assert.Contains(user.Active.ToString().ToLower(), await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateUserRoute_ValidParameter_NoErrors()
        {
            //Arrange
            var user = UserSetup();
            var createUserRequest = UserRequestSetup();
            CreateUserSetup(user);
            await using var application = new TestHostClass(_mockUserRepository.Object);
            var client = application.CreateClient();
            //Act
            var result = await client.PostAsJsonAsync(urlCreateUser, createUserRequest);
            //Assert
            Assert.Contains("\"errors\":null", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task CreateUserRoute_ValidParameter_IsErrorsFalse()
        {
            //Arrange
            var user = UserSetup();
            var createUserRequest = UserRequestSetup();
            CreateUserSetup(user);
            await using var application = new TestHostClass(_mockUserRepository.Object);
            var client = application.CreateClient();
            //Act
            var result = await client.PostAsJsonAsync(urlCreateUser, createUserRequest);
            //Assert
            Assert.Contains("\"isError\":false", await result.Content.ReadAsStringAsync());
        }

        #region Setup
        private void CreateUserSetup(UserEntity user)
        {
            _mockUserRepository.Setup(s => s.AddAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(user);
        }
        #endregion
    }
}
