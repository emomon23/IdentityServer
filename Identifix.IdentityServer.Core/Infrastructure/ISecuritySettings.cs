namespace Identifix.IdentityServer.Infrastructure
{
    public interface ISecuritySettings
    {
        int PasswordHashIterations { get; }
    }
}