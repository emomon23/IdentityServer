using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class ChangePasswordModel
    {
        public int UserId { get; set; }
        [Required]        
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Your new password must be at least 8 characters long.")]
        public string NewPassword { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Your new password must be at least 8 characters long.")]
        public string NewPasswordConfirm { get; set; }
    }
}