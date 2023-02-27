using AutoMapper;
using TP.NA.UserService.Application.Commands.User;

namespace TP.NA.UserService.Test.MapperConfig
{
    public static class AutoMapperConf
    {
        public static MapperConfiguration Config()
        {
            List<Profile> profileList = new List<Profile>
            {
                new UpdateUserCommand.Mapper()
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfiles(profileList));

            return config;
        }
    }
}