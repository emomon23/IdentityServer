using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public string SignInToken { get; set; }
    }
}