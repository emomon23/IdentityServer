using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Identifix.IdentityServer.Models.Data;

namespace Identifix.IdentityServer.Models.Data
{
    public class UserDatabaseInitializer : CreateDatabaseIfNotExists<SqlContext>
    {
        public override void InitializeDatabase(SqlContext context)
        {
            base.InitializeDatabase(context);
            context.Database.ExecuteSqlCommand(Properties.Resources.DBInitilizer);
        }

        protected override void Seed(SqlContext context)
        {
            IList<Country> countries = SeedCountries(context);
            Country us = countries.Single(c => c.Code == "US");
            IList<State> states = SeedStates(context, us);
            context.SaveChanges();
            SeedUsers(context, states);
            base.Seed(context);
        }

        private void SeedUsers(SqlContext context, IList<State>  states)
        {
            State state = states.SingleOrDefault(s => s.Code == "MN");
            Country country= state?.Country;
            Shop lakevilleShop = CreateShop("404", "Lakeville Performance Center", "1234 Ipava Path", "Lakeville", state, country, "55044");
            Shop farmingtonShop = CreateShop("420", "Farmington Performance Center", "5678 Elm Street", "Farmington", state, country, "55024");
            Shop burnsvilleShop = CreateShop("418", "Burnsville Performance Center", "9012 Nicollet Blvd", "Burnsville", state, country, "55037");
            UserAccount user1 = CreateUser("sly.juan@lpc.non", "Sly", "Juan", "", lakevilleShop);
            UserAccount user2 = CreateUser("slim.jim@lpc.non", "Slim", "Jim", "", lakevilleShop);
            UserAccount user3 = CreateUser("lefty.jones@lpc.non", "Left", "Jones", "", lakevilleShop);
            UserAccount user4 = CreateUser("tiny.stewart@fpc.non", "Tiny", "Stewart", "", farmingtonShop);
            UserAccount user5 = CreateUser("red.daniels@fpc.non", "Red", "Daniels", "", farmingtonShop);
            UserAccount user6 = CreateUser("fingers.malone@fpc.non", "Fingers", "Malone", "", farmingtonShop);
            UserAccount user7 = CreateUser("piston.pete@bpc.non", "Pete", "Piston", "", burnsvilleShop);
            UserAccount user8 = CreateUser("jerry.bomba@bpc.non", "Jerry", "Bomba", "", burnsvilleShop);
            UserAccount user9 = CreateUser("benny.alfonso@bpc.non", "Benny", "Alfonso", "", burnsvilleShop);
            List<UserAccount> users = new List<UserAccount> {user1, user2, user3, user4, user5, user6, user7, user8, user9};
            users.ForEach(user => context.Users.Add(user));
        }

        private static UserAccount CreateUser(string email, string firstName, string lastName, string password, Shop shop)
        {
            string pwd = (string.IsNullOrWhiteSpace(password)) ? "NONE" : password;
            UserAccount user1 = new UserAccount
            {
                Shop = shop,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = pwd
            };
            return user1;
        }

        private static Shop CreateShop(string shopId, string name, string address, string city, State state, Country country, string postalCode)
        {
            return new Shop
            {
                Address = new Address
                {
                    StateId = state.Id,
                    CountryId = country.Id,
                    City = city,
                    Line1 = address,
                    PostalCode = postalCode
                },
                Name = name,
                Users = new List<User>(),
                ShopId = shopId ,
            };
        }

        private IList<Country> SeedCountries(SqlContext context)
        {
            List<Country> countries = new List<Country>
            {
                new Country {Code = "AF", Name = "Afghanistan"},
                new Country {Code = "AX", Name = "Aland Islands"},
                new Country {Code = "AL", Name = "Albania"},
                new Country {Code = "DZ", Name = "Algeria"},
                new Country {Code = "AS", Name = "American Samoa"},
                new Country {Code = "AD", Name = "Andorra"},
                new Country {Code = "AO", Name = "Angola"},
                new Country {Code = "AI", Name = "Anguilla"},
                new Country {Code = "AQ", Name = "Antarctica"},
                new Country {Code = "AG", Name = "Antigua and Barbuda"},
                new Country {Code = "AR", Name = "Argentina"},
                new Country {Code = "AM", Name = "Armenia"},
                new Country {Code = "AW", Name = "Aruba"},
                new Country {Code = "AU", Name = "Australia"},
                new Country {Code = "AT", Name = "Austria"},
                new Country {Code = "AZ", Name = "Azerbaijan"},
                new Country {Code = "BS", Name = "Bahamas"},
                new Country {Code = "BH", Name = "Bahrain"},
                new Country {Code = "BD", Name = "Bangladesh"},
                new Country {Code = "BB", Name = "Barbados"},
                new Country {Code = "BY", Name = "Belarus"},
                new Country {Code = "BE", Name = "Belgium"},
                new Country {Code = "BZ", Name = "Belize"},
                new Country {Code = "BJ", Name = "Benin"},
                new Country {Code = "BM", Name = "Bermuda"},
                new Country {Code = "BT", Name = "Bhutan"},
                new Country {Code = "BO", Name = "Bolivia, Plurinational State of"},
                new Country {Code = "BQ", Name = "Bonaire, Sint Eustatius and Saba"},
                new Country {Code = "BA", Name = "Bosnia and Herzegovina"},
                new Country {Code = "BW", Name = "Botswana"},
                new Country {Code = "BV", Name = "Bouvet Island"},
                new Country {Code = "BR", Name = "Brazil"},
                new Country {Code = "IO", Name = "British Indian Ocean Territory"},
                new Country {Code = "BN", Name = "Brunei Darussalam"},
                new Country {Code = "BG", Name = "Bulgaria"},
                new Country {Code = "BF", Name = "Burkina Faso"},
                new Country {Code = "BI", Name = "Burundi"},
                new Country {Code = "KH", Name = "Cambodia"},
                new Country {Code = "CM", Name = "Cameroon"},
                new Country {Code = "CA", Name = "Canada"},
                new Country {Code = "CV", Name = "Cape Verde"},
                new Country {Code = "KY", Name = "Cayman Islands"},
                new Country {Code = "CF", Name = "Central African Republic"},
                new Country {Code = "TD", Name = "Chad"},
                new Country {Code = "CL", Name = "Chile"},
                new Country {Code = "CN", Name = "China"},
                new Country {Code = "CX", Name = "Christmas Island"},
                new Country {Code = "CC", Name = "Cocos (Keeling) Islands"},
                new Country {Code = "CO", Name = "Colombia"},
                new Country {Code = "KM", Name = "Comoros"},
                new Country {Code = "CG", Name = "Congo"},
                new Country {Code = "CD", Name = "Congo, the Democratic Republic of the"},
                new Country {Code = "CK", Name = "Cook Islands"},
                new Country {Code = "CR", Name = "Costa Rica"},
                new Country {Code = "CI", Name = "Cote d'Ivoire"},
                new Country {Code = "HR", Name = "Croatia"},
                new Country {Code = "CU", Name = "Cuba"},
                new Country {Code = "CW", Name = "Curacao"},
                new Country {Code = "CY", Name = "Cyprus"},
                new Country {Code = "CZ", Name = "Czech Republic"},
                new Country {Code = "DK", Name = "Denmark"},
                new Country {Code = "DJ", Name = "Djibouti"},
                new Country {Code = "DM", Name = "Dominica"},
                new Country {Code = "DO", Name = "Dominican Republic"},
                new Country {Code = "EC", Name = "Ecuador"},
                new Country {Code = "EG", Name = "Egypt"},
                new Country {Code = "SV", Name = "El Salvador"},
                new Country {Code = "GQ", Name = "Equatorial Guinea"},
                new Country {Code = "ER", Name = "Eritrea"},
                new Country {Code = "EE", Name = "Estonia"},
                new Country {Code = "ET", Name = "Ethiopia"},
                new Country {Code = "FK", Name = "Falkland Islands (Malvinas)"},
                new Country {Code = "FO", Name = "Faroe Islands"},
                new Country {Code = "FJ", Name = "Fiji"},
                new Country {Code = "FI", Name = "Finland"},
                new Country {Code = "FR", Name = "France"},
                new Country {Code = "GF", Name = "French Guiana"},
                new Country {Code = "PF", Name = "French Polynesia"},
                new Country {Code = "TF", Name = "French Southern Territories"},
                new Country {Code = "GA", Name = "Gabon"},
                new Country {Code = "GM", Name = "Gambia"},
                new Country {Code = "GE", Name = "Georgia"},
                new Country {Code = "DE", Name = "Germany"},
                new Country {Code = "GH", Name = "Ghana"},
                new Country {Code = "GI", Name = "Gibraltar"},
                new Country {Code = "GR", Name = "Greece"},
                new Country {Code = "GL", Name = "Greenland"},
                new Country {Code = "GD", Name = "Grenada"},
                new Country {Code = "GP", Name = "Guadeloupe"},
                new Country {Code = "GU", Name = "Guam"},
                new Country {Code = "GT", Name = "Guatemala"},
                new Country {Code = "GG", Name = "Guernsey"},
                new Country {Code = "GN", Name = "Guinea"},
                new Country {Code = "GW", Name = "Guinea-Bissau"},
                new Country {Code = "GY", Name = "Guyana"},
                new Country {Code = "HT", Name = "Haiti"},
                new Country {Code = "HM", Name = "Heard Island and McDonald Islands"},
                new Country {Code = "VA", Name = "Holy See (Vatican City State)"},
                new Country {Code = "HN", Name = "Honduras"},
                new Country {Code = "HK", Name = "Hong Kong"},
                new Country {Code = "HU", Name = "Hungary"},
                new Country {Code = "IS", Name = "Iceland"},
                new Country {Code = "IN", Name = "India"},
                new Country {Code = "ID", Name = "Indonesia"},
                new Country {Code = "IR", Name = "Iran, Islamic Republic of"},
                new Country {Code = "IQ", Name = "Iraq"},
                new Country {Code = "IE", Name = "Ireland"},
                new Country {Code = "IM", Name = "Isle of Man"},
                new Country {Code = "IL", Name = "Israel"},
                new Country {Code = "IT", Name = "Italy"},
                new Country {Code = "JM", Name = "Jamaica"},
                new Country {Code = "JP", Name = "Japan"},
                new Country {Code = "JE", Name = "Jersey"},
                new Country {Code = "JO", Name = "Jordan"},
                new Country {Code = "KZ", Name = "Kazakhstan"},
                new Country {Code = "KE", Name = "Kenya"},
                new Country {Code = "KI", Name = "Kiribati"},
                new Country {Code = "KP", Name = "Korea, Democratic People's Republic of"},
                new Country {Code = "KR", Name = "Korea, Republic of"},
                new Country {Code = "KW", Name = "Kuwait"},
                new Country {Code = "KG", Name = "Kyrgyzstan"},
                new Country {Code = "LA", Name = "Lao People's Democratic Republic"},
                new Country {Code = "LV", Name = "Latvia"},
                new Country {Code = "LB", Name = "Lebanon"},
                new Country {Code = "LS", Name = "Lesotho"},
                new Country {Code = "LR", Name = "Liberia"},
                new Country {Code = "LY", Name = "Libya"},
                new Country {Code = "LI", Name = "Liechtenstein"},
                new Country {Code = "LT", Name = "Lithuania"},
                new Country {Code = "LU", Name = "Luxembourg"},
                new Country {Code = "MO", Name = "Macao"},
                new Country {Code = "MK", Name = "Macedonia, The Former Yugoslav Republic of"},
                new Country {Code = "MG", Name = "Madagascar"},
                new Country {Code = "MW", Name = "Malawi"},
                new Country {Code = "MY", Name = "Malaysia"},
                new Country {Code = "MV", Name = "Maldives"},
                new Country {Code = "ML", Name = "Mali"},
                new Country {Code = "MT", Name = "Malta"},
                new Country {Code = "MH", Name = "Marshall Islands"},
                new Country {Code = "MQ", Name = "Martinique"},
                new Country {Code = "MR", Name = "Mauritania"},
                new Country {Code = "MU", Name = "Mauritius"},
                new Country {Code = "YT", Name = "Mayotte"},
                new Country {Code = "MX", Name = "Mexico"},
                new Country {Code = "FM", Name = "Micronesia, Federated States of"},
                new Country {Code = "MD", Name = "Moldova, Republic of"},
                new Country {Code = "MC", Name = "Monaco"},
                new Country {Code = "MN", Name = "Mongolia"},
                new Country {Code = "ME", Name = "Montenegro"},
                new Country {Code = "MS", Name = "Montserrat"},
                new Country {Code = "MA", Name = "Morocco"},
                new Country {Code = "MZ", Name = "Mozambique"},
                new Country {Code = "MM", Name = "Myanmar"},
                new Country {Code = "NA", Name = "Namibia"},
                new Country {Code = "NR", Name = "Nauru"},
                new Country {Code = "NP", Name = "Nepal"},
                new Country {Code = "NL", Name = "Netherlands"},
                new Country {Code = "NC", Name = "New Caledonia"},
                new Country {Code = "NZ", Name = "New Zealand"},
                new Country {Code = "NI", Name = "Nicaragua"},
                new Country {Code = "NE", Name = "Niger"},
                new Country {Code = "NG", Name = "Nigeria"},
                new Country {Code = "NU", Name = "Niue"},
                new Country {Code = "NF", Name = "Norfolk Island"},
                new Country {Code = "MP", Name = "Northern Mariana Islands"},
                new Country {Code = "NO", Name = "Norway"},
                new Country {Code = "OM", Name = "Oman"},
                new Country {Code = "PK", Name = "Pakistan"},
                new Country {Code = "PW", Name = "Palau"},
                new Country {Code = "PS", Name = "Palestinian Territory, Occupied"},
                new Country {Code = "PA", Name = "Panama"},
                new Country {Code = "PG", Name = "Papua New Guinea"},
                new Country {Code = "PY", Name = "Paraguay"},
                new Country {Code = "PE", Name = "Peru"},
                new Country {Code = "PH", Name = "Philippines"},
                new Country {Code = "PN", Name = "Pitcairn"},
                new Country {Code = "PL", Name = "Poland"},
                new Country {Code = "PT", Name = "Portugal"},
                new Country {Code = "PR", Name = "Puerto Rico"},
                new Country {Code = "QA", Name = "Qatar"},
                new Country {Code = "RE", Name = "Reunion"},
                new Country {Code = "RO", Name = "Romania"},
                new Country {Code = "RU", Name = "Russian Federation"},
                new Country {Code = "RW", Name = "Rwanda"},
                new Country {Code = "BL", Name = "Saint Barthelemy"},
                new Country {Code = "SH", Name = "Saint Helena, Ascension and Tristan da Cunha"},
                new Country {Code = "KN", Name = "Saint Kitts and Nevis"},
                new Country {Code = "LC", Name = "Saint Lucia"},
                new Country {Code = "MF", Name = "Saint Martin (French part)"},
                new Country {Code = "PM", Name = "Saint Pierre and Miquelon"},
                new Country {Code = "VC", Name = "Saint Vincent and the Grenadines"},
                new Country {Code = "WS", Name = "Samoa"},
                new Country {Code = "SM", Name = "San Marino"},
                new Country {Code = "ST", Name = "Sao Tome and Principe"},
                new Country {Code = "SA", Name = "Saudi Arabia"},
                new Country {Code = "SN", Name = "Senegal"},
                new Country {Code = "RS", Name = "Serbia"},
                new Country {Code = "SC", Name = "Seychelles"},
                new Country {Code = "SL", Name = "Sierra Leone"},
                new Country {Code = "SG", Name = "Singapore"},
                new Country {Code = "SX", Name = "Sint Maarten (Dutch part)"},
                new Country {Code = "SK", Name = "Slovakia"},
                new Country {Code = "SI", Name = "Slovenia"},
                new Country {Code = "SB", Name = "Solomon Islands"},
                new Country {Code = "SO", Name = "Somalia"},
                new Country {Code = "ZA", Name = "South Africa"},
                new Country {Code = "GS", Name = "South Georgia and the South Sandwich Islands"},
                new Country {Code = "SS", Name = "South Sudan"},
                new Country {Code = "ES", Name = "Spain"},
                new Country {Code = "LK", Name = "Sri Lanka"},
                new Country {Code = "SD", Name = "Sudan"},
                new Country {Code = "SR", Name = "Suriname"},
                new Country {Code = "SJ", Name = "Svalbard and Jan Mayen"},
                new Country {Code = "SZ", Name = "Swaziland"},
                new Country {Code = "SE", Name = "Sweden"},
                new Country {Code = "CH", Name = "Switzerland"},
                new Country {Code = "SY", Name = "Syrian Arab Republic"},
                new Country {Code = "TW", Name = "Taiwan, Province of China"},
                new Country {Code = "TJ", Name = "Tajikistan"},
                new Country {Code = "TZ", Name = "Tanzania, United Republic of"},
                new Country {Code = "TH", Name = "Thailand"},
                new Country {Code = "TL", Name = "Timor-Leste"},
                new Country {Code = "TG", Name = "Togo"},
                new Country {Code = "TK", Name = "Tokelau"},
                new Country {Code = "TO", Name = "Tonga"},
                new Country {Code = "TT", Name = "Trinidad and Tobago"},
                new Country {Code = "TN", Name = "Tunisia"},
                new Country {Code = "TR", Name = "Turkey"},
                new Country {Code = "TM", Name = "Turkmenistan"},
                new Country {Code = "TC", Name = "Turks and Caicos Islands"},
                new Country {Code = "TV", Name = "Tuvalu"},
                new Country {Code = "UG", Name = "Uganda"},
                new Country {Code = "UA", Name = "Ukraine"},
                new Country {Code = "AE", Name = "United Arab Emirates"},
                new Country {Code = "GB", Name = "United Kingdom"},
                new Country {Code = "US", Name = "United States"},
                new Country {Code = "UM", Name = "United States Minor Outlying Islands"},
                new Country {Code = "UY", Name = "Uruguay"},
                new Country {Code = "UZ", Name = "Uzbekistan"},
                new Country {Code = "VU", Name = "Vanuatu"},
                new Country {Code = "VE", Name = "Venezuela, Bolivarian Republic of"},
                new Country {Code = "VN", Name = "Viet Nam"},
                new Country {Code = "VG", Name = "Virgin Islands, British"},
                new Country {Code = "VI", Name = "Virgin Islands, U.S."},
                new Country {Code = "WF", Name = "Wallis and Futuna"},
                new Country {Code = "EH", Name = "Western Sahara"},
                new Country {Code = "YE", Name = "Yemen"},
                new Country {Code = "ZM", Name = "Zambia"},
                new Country {Code = "ZW", Name = "Zimbabwe"}
            };
            countries.ForEach(country=> context.Countries.Add(country));
            return countries;
        }

        private IList<State> SeedStates(SqlContext context, Country us)
        {
            us.States = new List<State>
            {
                new State {Code = "AL", Name = "Alabama"},
                new State {Code = "AK", Name = "Alaska"},
                new State {Code = "AS", Name = "American Samoa"},
                new State {Code = "AZ", Name = "Arizona"},
                new State {Code = "AR", Name = "Arkansas"},
                new State {Code = "CA", Name = "California"},
                new State {Code = "CO", Name = "Colorado"},
                new State {Code = "CT", Name = "Connecticut"},
                new State {Code = "DE", Name = "Delaware"},
                new State {Code = "DC", Name = "District of Columbia"},
                new State {Code = "FM", Name = "Federated States of Micronesia"},
                new State {Code = "FL", Name = "Florida"},
                new State {Code = "GA", Name = "Georgia"},
                new State {Code = "GU", Name = "Guam"},
                new State {Code = "HI", Name = "Hawaii"},
                new State {Code = "ID", Name = "Idaho"},
                new State {Code = "IL", Name = "Illinois"},
                new State {Code = "IN", Name = "Indiana"},
                new State {Code = "IA", Name = "Iowa"},
                new State {Code = "KS", Name = "Kansas"},
                new State {Code = "KY", Name = "Kentucky"},
                new State {Code = "LA", Name = "Louisiana"},
                new State {Code = "ME", Name = "Maine"},
                new State {Code = "MH", Name = "Marshall Islands"},
                new State {Code = "MD", Name = "Maryland"},
                new State {Code = "MA", Name = "Massachusetts"},
                new State {Code = "MI", Name = "Michigan"},
                new State {Code = "MN", Name = "Minnesota"},
                new State {Code = "MS", Name = "Mississippi"},
                new State {Code = "MO", Name = "Missouri"},
                new State {Code = "MT", Name = "Montana"},
                new State {Code = "NE", Name = "Nebraska"},
                new State {Code = "NV", Name = "Nevada"},
                new State {Code = "NH", Name = "New Hampshire"},
                new State {Code = "NJ", Name = "New Jersey"},
                new State {Code = "NM", Name = "New Mexico"},
                new State {Code = "NY", Name = "New York"},
                new State {Code = "NC", Name = "North Carolina"},
                new State {Code = "ND", Name = "North Dakota"},
                new State {Code = "MP", Name = "Northern Mariana Islands"},
                new State {Code = "OH", Name = "Ohio"},
                new State {Code = "OK", Name = "Oklahoma"},
                new State {Code = "OR", Name = "Oregon"},
                new State {Code = "PW", Name = "Palau"},
                new State {Code = "PA", Name = "Pennsylvania"},
                new State {Code = "PR", Name = "Puerto Rico"},
                new State {Code = "RI", Name = "Rhode Island"},
                new State {Code = "SC", Name = "South Carolina"},
                new State {Code = "SD", Name = "South Dakota"},
                new State {Code = "TN", Name = "Tennessee"},
                new State {Code = "TX", Name = "Texas"},
                new State {Code = "UT", Name = "Utah"},
                new State {Code = "VT", Name = "Vermont"},
                new State {Code = "VI", Name = "Virgin Islands"},
                new State {Code = "VA", Name = "Virginia"},
                new State {Code = "WA", Name = "Washington"},
                new State {Code = "WV", Name = "West Virginia"},
                new State {Code = "WI", Name = "Wisconsin"},
                new State {Code = "WY", Name = "Wyoming"},
                new State {Code = "AE", Name = "Armed Forces Europe, the Middle East, and Canada"},
                new State {Code = "AP", Name = "Armed Forces Pacific"},
                new State {Code = "AA", Name = "Armed Forces Americas "}
            };

            return us.States.ToList();
        }
    }
}