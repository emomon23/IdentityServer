using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer.Models
{
    public class ResetPasswordLink
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [Required, StringLength(36, MinimumLength = 36)]        
        public string Token { get; set; }
        [Required]
        public DateTime Expiration { get; set; }
    }
}
