using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identifix.IdentityServer.Models
{
    public class User
    {
        public User()
        {
            this.Shop = new Shop();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(64)]
        public string FirstName { get; set; }

        [StringLength(64)]
        public string LastName { get; set; }

        [StringLength(384, MinimumLength = 6)]
        public string Email { get; set; }

        [ForeignKey("Shop")]
        public int ShopId { get; set; }

        public Shop Shop { get; set; }
    }
}