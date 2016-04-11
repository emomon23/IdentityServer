using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identifix.IdentityServer.Models
{

    public class State
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Required, StringLength(64)]
        public virtual string Name { get; set; }

        [Required, StringLength(6)]
        public virtual string Code { get; set; }

        [Required, ForeignKey("Country")]
        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}