using System.Net.Http.Headers;
using TP.NA.Common.Repository.Extensions;
using TP.NA.Common.Repository.Persistance;
using TP.NA.UserService.Application.Abstractions.Repositories;
using TP.NA.UserService.Application.Exceptions;
using TP.NA.UserService.Domain.Entities;

namespace TP.NA.UserService.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ICosmosRepository<UserEntity> _userCosmosRepository;

        public UserRepository(ICosmosRepository<UserEntity> cosmosRepository)
        {
            _userCosmosRepository = cosmosRepository;
        }

        public async Task<UserEntity> AddAsync(UserEntity user)
        {
            var result = await _userCosmosRepository.Upsert(user);
            if (result.StatusCode != System.Net.HttpStatusCode.Created)
                return null;
            return result.Model;
        }

        public async Task<UserEntity> GetByNameAsync(string userName)
        {
            var user = await (await _userCosmosRepository.Get()).Where(x => x.Name.ToUpper() == userName.ToUpper()).LinqQueryToResults();
            return user.FirstOrDefault();
        }

        public async Task<UserEntity?> GetByIdAsync(string id)
        {
            var result = (await (await _userCosmosRepository.Get(id)).LinqQueryToResults()).FirstOrDefault();

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<UserEntity> UpdateAsync(UserEntity user)
        {
            var result = await _userCosmosRepository.Upsert(user);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new CosmosDBError("Error with DB, review logs");
            }

            return result.Model;
        }
    }
}