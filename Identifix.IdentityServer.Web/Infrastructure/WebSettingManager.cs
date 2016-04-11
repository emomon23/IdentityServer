using System;
using System.Configuration;
using Identifix.IdentityServer.Constants;

namespace Identifix.IdentityServer.Infrastructure
{
    public class WebSettingManager : ISettingManager
    {
        public string UserDatabaseConnectionString => ConfigurationManager.ConnectionStrings[SettingKey.UserDatabaseConnection].ConnectionString;
        public int PasswordHashIterations => Convert.ToInt32(ConfigurationManager.AppSettings[SettingKey.PasswordHashIterations]);
    }
}