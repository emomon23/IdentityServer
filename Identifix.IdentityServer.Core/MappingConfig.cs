using AutoMapper;
using Identifix.IdentityServer.Models;

namespace Identifix.IdentityServer
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<UserAccount, User>();
            Mapper.CreateMap<User, UserAccount>();
        }
    }
}