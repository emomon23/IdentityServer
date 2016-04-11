using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identifix.IdentityServer.Models
{
    public class Shop
    {
        public Shop()
        {
            this.Address = new Address();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ShopId { get; set; }

        [Required, StringLength(64)]
        public virtual string Name { get; set; }

        public ICollection<User> Users { get; set; }

        [Required, ForeignKey("Address")]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}