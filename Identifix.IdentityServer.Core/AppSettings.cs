using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer
{
    public static class AppSettings
    {
        internal static string IdentifixSSOUrl => "https://localhost:44300/";

        /// <summary>
        /// The number of minutes to add to the current time to set a expiration time.
        /// </summary>
        public static int PasswordResetExpirationAmount => 15;

        public static string TemporaryEmailFolder => $"{System.AppDomain.CurrentDomain.BaseDirectory}Emails";
        /// <summary>
        /// Email address that is used in the FROM field for system generated emails.
        /// </summary>
        public static string IdentifixSystemEmailAddress => "Testing@Identifix.com";
    }
}
