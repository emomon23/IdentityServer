using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class NewUserRegistration : UserProfile
    {
        [Required, StringLength(128), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}