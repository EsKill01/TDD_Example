using TP.NA.UserService.Application.Models;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Test.UserTest
{
    public static class UserMockData
    {
        public static ICollection<UserEntity> GetUserEntities
        {
            get
            {
                return new List<UserEntity>()
                {
                    new UserEntity
                    {
                        Id = "1234"
                    },
                    new UserEntity
                    {
                    },
                    new UserEntity
                    {
                    }
                };
            }
        }

        public static ICollection<UserModel> GetUserModels
        {
            get
            {
                return new List<UserModel>()
                {
                    new UserModel
                    {
                    },
                    new UserModel
                    {
                    },
                    new UserModel
                    {
                    }
                };
            }
        }
    }
}