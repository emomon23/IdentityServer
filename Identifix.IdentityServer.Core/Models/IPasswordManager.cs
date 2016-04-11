namespace Identifix.IdentityServer.Models
{
    public interface IPasswordManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string expectedHash);
    }
}