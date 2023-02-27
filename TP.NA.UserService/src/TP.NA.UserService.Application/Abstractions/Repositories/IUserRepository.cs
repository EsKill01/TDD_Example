using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> AddAsync(UserEntity user);
        Task<UserEntity> GetByNameAsync(string name);
        Task<UserEntity> UpdateAsync(UserEntity user);
        Task<UserEntity> GetByIdAsync(string id);
    }
}