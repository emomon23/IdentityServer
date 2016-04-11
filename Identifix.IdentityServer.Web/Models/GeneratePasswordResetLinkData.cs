using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class GeneratePasswordResetLinkData
    {
        public string ClientIdRedirect { get; set; }
        public string UsersEmailAddress { get; set; }
    }
}