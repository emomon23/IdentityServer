using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation.HelperObjects;
using iEmosoft;
using iEmosoft.Automation;
using Identifix.IdentityServer.Automation.DataState.Model;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models.Data;

namespace Identifix.IdentityServer.Automation.DataState
{
    public class AuthenticationDataState
    {
        RandomTestData randomData = new RandomTestData();
        private TestExecutioner executioner;
        SqlContext db = null;

        private TestUser inputUser;

        public AuthenticationDataState(TestExecutioner executioner)
        {
            try
            {
                this.executioner = executioner;
                db = new SqlContext(new Settings());
            }
            catch (Exception exp)
            {
                HandleException(exp, ":Ctor", "To create a new SqlContext");
            }
        }

        public TestUser RetrieveInputUserFromDBOtherThanThese(params string[] emailAddresses)
        {
            
            string inClause = string.Join(",", emailAddresses);
            var dbUser =
                   db.Users.Include("Shop").Include("Shop.Address").FirstOrDefault(u => inClause.Contains(u.Email) == false);

            if (dbUser == null)
            {
                throw new Exception("Unable to find a user in the db other than " + inClause);
            }

            TestUser result = new TestUser()
            {
                FirstName = dbUser.FirstName,
                LastName = dbUser.LastName,
                ShopName = dbUser.Shop.Name,
                Email = dbUser.Email,
                Address1 = dbUser.Shop.Address.Line1,
                Address2 = dbUser.Shop.Address.Line2,
                City = dbUser.Shop.Address.City,
                StateId = dbUser.Shop.Address.StateId,
                CountryId = dbUser.Shop.Address.CountryId
            };

            return result;
        }

        public TestUser RetrieveInputUserFromDB()
        {
            try
            {
                TestUser result = null;

                var dbUser = db.Users.Include("Shop").Include("Shop.Address").FirstOrDefault();

                if (dbUser != null)
                {
                    result = new TestUser()
                    {
                        FirstName = dbUser.FirstName,
                        LastName = dbUser.LastName,
                        ShopName = dbUser.Shop.Name,
                        Email = dbUser.Email,
                        Address1 = dbUser.Shop.Address.Line1,
                        Address2 = dbUser.Shop.Address.Line2,
                        City = dbUser.Shop.Address.City,
                        StateId = dbUser.Shop.Address.StateId,
                        CountryId = dbUser.Shop.Address.CountryId,
                        Password = "P@ssword23!"

                    };
                }

                return result;
            }
            catch (Exception exp)
            {
                HandleException(exp, "RetrieveInputUserFromDB()", "To retrieve an existing user");
                return null;
            }
        }



        public TestUser RetrieveInputUserFromDB(string emailAddress)
        {
            try
            {
                TestUser result = null;

                var dbUser =
                    db.Users.Include("Shop").Include("Shop.Address").FirstOrDefault(u => u.Email == emailAddress);

                if (dbUser != null)
                {
                    result = new TestUser()
                    {
                        FirstName = dbUser.FirstName,
                        LastName = dbUser.LastName,
                        ShopName = dbUser.Shop.Name,
                        Email = dbUser.Email,
                        Address1 = dbUser.Shop.Address.Line1,
                        Address2 = dbUser.Shop.Address.Line2,
                        City = dbUser.Shop.Address.City,
                        StateId = dbUser.Shop.Address.StateId,
                        CountryId = dbUser.Shop.Address.CountryId,
                        Password = "P@ssword23!"

                    };
                }

                return result;
            }
            catch (Exception exp)
            {
                HandleException(exp, "RetrieveInputUserFromDB(email)", "To retrieve an existing user");
                return null;
            }
        }


        public TestUser InputUser
        {
            get
            {
                return inputUser;
                
            }
        }
        
        public TestUser CreateRandomNewUserObject()
        {
            var person = randomData.GetRandomPerson();
            this.inputUser = new TestUser();
            inputUser.FirstName = person.FirstName;
            inputUser.LastName = person.LastName;
            inputUser.Email = person.EmailAddress.Replace(",", "");
            inputUser.ConfirmEmail = inputUser.Email;
            inputUser.ShopName = randomData.GetRandomCompanyName();
            inputUser.City = person.HomeAddress.City;
            inputUser.Address1 = person.HomeAddress.Street1;
            inputUser.CountryId = 1;
            inputUser.StateId = 2;
            inputUser.ZipCode = person.HomeAddress.PostalCode;
            inputUser.Password = "P@ssword12!";
            inputUser.ConfirmPassword = inputUser.Password;


            inputUser.StateId = int.Parse(
                randomData.GetRandomValueFromArray(new List<string>
                {
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                    "10",
                    "21",
                    "31",
                    "40"
                }));

            return inputUser;
            
        }

        private void HandleException(Exception exp, string source, string expectedResult)
        {
            string actualResult = "Error: " + exp.Message;
            if (executioner.CurrentStep == null)
            {
                executioner.BeginTestCaseStep("AuthenticationDataSet." + source, expectedResult, actualResult, false);
            }
            executioner.FailCurrentStep(expectedResult, actualResult + " - " + exp.Message, true);

            throw new Exception(source + " " + expectedResult + " " + actualResult, exp);
        }
   }

    public class Settings : ISettingManager
    {
        public string UserDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString; }
        }

        public int PasswordHashIterations { get { return 0; } }
    }
}
