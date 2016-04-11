using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection.Emit;

namespace Identifix.IdentityServer.Models.Data
{
    /// <summary>
    /// Handles storage and retrieval of User related Information
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Returns a list of all know countries from the datastore
        /// </summary>
        /// <returns></returns>
        IList<Country> RetrieveCountries();

        /// <summary>
        /// Returns a list of all states belongiong to the specified country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        IList<State> RetrieveStates(int countryId);

        /// <summary>
        /// Returns the entire list of states
        /// </summary>
        /// <returns></returns>
        IList<State> RetrieveStates();

        /// <summary>
        /// Returns a list of all states belongiong to the specified country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        IList<State> RetrieveStates(string countryCode);

        /// <summary>
        /// Checks to see if there is an existin guser with the spceified e-mail address.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        bool CheckIfEmailExists(string emailAddress);

        /// <summary>
        /// Attempts to find a shop based on an address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Shop RetrieveShop(Address address);

        /// <summary>
        /// Saves a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserAccount SaveUser(UserAccount user);

        /// <summary>
        /// Retrieves the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserAccount RetrieveUser(int id);

        /// <summary>
        /// Retrieves the user
        /// </summary>
        /// <param name="email"></param>
        UserAccount RetrieveUser(string email);
        string SaveResetPasswordData(int userId);
        int? IsValidToken(string token);
        string PasswordResetLinkExistsForUser(int userId);
        void ExpireResetPasswordTokenForUser(int userId);
    }
}