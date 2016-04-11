using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identifix.IdentityServer.Models
{
    public class Address
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Required, StringLength(128)]
        public virtual string Line1 { get; set; }
        
        [StringLength(128)]
        public virtual string Line2 { get; set; }

        [Required, StringLength(64)]
        public virtual string City { get; set; }

        [ForeignKey("State")]
        public virtual int StateId { get; set; }

        public virtual State State { get; set; }

        [Required, ForeignKey("Country")]
        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Required, StringLength(16)]
        public virtual string PostalCode { get; set; }
    }
}