using log4net;

namespace Identifix.IdentityServer.Infrastructure
{
    public interface IApplicationContext
    {
        ISettingManager Settings { get; }
        IStateManager State { get; }
        ILog Logger { get; }
    }
}