using System.Collections.Generic;

namespace Identifix.IdentityServer.Models
{
    public interface IUserService
    {
        IList<Country> GetCountries();
        IList<State> GetStates(string countryCode);
        IList<State> GetStates(int countryId);
        IList<State> GetStates();
        bool VerifyEmailAddressIsAvaliable(string emailAddress);
        User RegisterNewUser(User user, string password);
        User RetrieveUserProfile(int id);
        void UpdateUserProfile(User userProfile);
        bool UpdatePasswordForUser(int userId, string currentPassword, string newPassword, string newPasswordConfirm);
        string GeneratePasswordResetLink(string emailAddress, string redirectSignInToken);
        bool SavePasswordFromReset(string token, string newPassword, string newPasswordConfirm);
        bool VerifyPasswordResetToken(string token);
    }
}