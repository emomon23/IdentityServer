using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer.Automation.DataState.Model
{
    public class TestUser
    {

        public string Email { get; set; }

        public string ConfirmEmail { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string ShopName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public string ZipCode { get; set; }

    }
}
