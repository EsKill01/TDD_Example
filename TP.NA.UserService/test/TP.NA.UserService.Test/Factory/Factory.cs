using AutoMapper;
using MediatR;
using Moq;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Configs;

namespace TP.NA.UserService.Test.Factory
{
    public class FactoryCommon
    {
        #region Mocks
        public static Mock<IUserRepository> MockIUserRepository => new();
        public static Mock<IMediator> MockMediator = new();
        #endregion 
        #region CancellationToken
        public static CancellationToken CancellationToken => new CancellationTokenSource().Token;
        #endregion
        #region Mappers
        public static Mapper mapperUser => new Mapper(new MapperConfiguration(config => config.AddProfile(new AutoMapperConfig())));
        #endregion
        #region Response

        #endregion
    }
}
