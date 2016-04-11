using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using IdentityServer3.Core;

namespace Identifix.IdentityServer.Models
{
    public class UserAccount :User
    {
        [Required, StringLength(128), DataType(DataType.Password)]
        public string Password { get; set; }

    }
}