using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
    }
}