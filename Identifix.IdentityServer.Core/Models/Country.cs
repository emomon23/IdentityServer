using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identifix.IdentityServer.Models
{
    public class Country
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        
        [Required, StringLength(128)]
        public virtual string Name { get; set; } 

        [Required, StringLength(2, MinimumLength = 2)]
        public virtual string Code { get; set; }

        [ForeignKey("Id")]
        public virtual ICollection<State> States { get; set; }
    }
}