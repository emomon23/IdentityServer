using log4net;
using System.Diagnostics.Contracts;

namespace Identifix.IdentityServer.Infrastructure
{
    public class WebApplicationContext : IApplicationContext
    {
        public WebApplicationContext(ISettingManager settings, IStateManager state, ILog logger)
        {
            Guard.IsNotNull(settings, "settings");
            Guard.IsNotNull(state, "state");
            Guard.IsNotNull(logger, "logger");
            Settings = settings;
            State = state;
            Logger = logger;
        }

        public ISettingManager Settings { get; }
        public IStateManager State { get; }
        public ILog Logger { get; }
    }
}