using System.IO.MemoryMappedFiles;
using AutoMapper;
using Identifix.IdentityServer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class MappingConfigTests
    {
        [TestInitialize]
        public void Intialize()
        {
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            
        }

        [TestMethod]
        public void CreateMappings_AddsMappingForUserToUserAccount()
        {
            MappingConfig.Configure();
            TypeMap map = Mapper.Instance.ConfigurationProvider.FindTypeMapFor<User, UserAccount>();

            Assert.IsNotNull(map);
        }
        
        [TestMethod]
        public void CreateMappings_AddsMappingForUserAccountToUser()
        {
            MappingConfig.Configure();
            TypeMap map = Mapper.Instance.ConfigurationProvider.FindTypeMapFor<UserAccount, User>();

            Assert.IsNotNull(map);
        }
    }
}