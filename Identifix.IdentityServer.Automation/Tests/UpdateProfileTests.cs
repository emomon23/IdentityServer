using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.QualityTools.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iEmosoft.Automation;
using iEmosoft.Automation.BaseClasses;
using iEmosoft.Automation.Model;
using iEmosoft.Automation.HelperObjects;
using Identifix.IdentityServer.Automation.DataState;
using Identifix.IdentityServer.Automation.DataState.Model;
using Identifix.IdentityServer.Automation.Features;

namespace Identifix.IdentityServer.Automation.Tests
{
    [TestClass]
    public class UpdateProfileTests : BaseTestClass
    {
        private string profileUpdateFamily = "Update Profile";
        private static TestUser existingUser = null;
        
        [TestMethod]
        public void UI_UpPro10_Successfully()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = profileUpdateFamily,
                TestDescription =
                    "Given an existing user updates their profile, When all the data is provided as expected.  Then their account should be updated.",
                TestName = "Update Profile Successfully",
                TestNumber = "UI_UpPro10"
            };

            using (var app = this.CreateSSOApp(testCaseDescription))
            {
                TestUser updatedUserData = app.DataStateManager.CreateRandomNewUserObject();

                app.UpdateProfileFeature.PostUpdateData(updatedUserData, true);
                app.NavigationFeature.AssertCurrentPage(NavigationFeature.PageNameEnumerations.DataSavedPage);

                existingUser = updatedUserData;
            }
        }

        [TestMethod]
        public void UI_UpPro11_RequiredFieldsValidation()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = profileUpdateFamily,
                TestDescription =
                    "Given an existing user updates their profile, When they haven't provided any data and submits the form, Then 'Required' validation messages should display and the data not submitted to the server.",
                TestName = "Required Field Validation",
                TestNumber = "UI_UpPro11"
            };

            using (var app = this.CreateSSOApp(testCaseDescription))
            {
                app.UpdateProfileFeature.ClearUpdateProfileForm(true);
                app.UpdateProfileFeature.AssertExpectedValidationsAreDisplaying(
                    UpdateProfileFeature.UpdateProfileValidations.address1Required,
                    UpdateProfileFeature.UpdateProfileValidations.cityRequired,
                    UpdateProfileFeature.UpdateProfileValidations.confirmEmailRequired,
                    UpdateProfileFeature.UpdateProfileValidations.emailRequired,
                    UpdateProfileFeature.UpdateProfileValidations.firstNameRequired,
                    UpdateProfileFeature.UpdateProfileValidations.lastNameRequired,
                    UpdateProfileFeature.UpdateProfileValidations.shopNameRequired,
                    UpdateProfileFeature.UpdateProfileValidations.zipRequired);

            }
        }

        [TestMethod]
        public void UI_UpPro12_DuplicateEmailValidation()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = profileUpdateFamily,
                TestDescription =
                    "Given an existing user updates their profile, If they change their email to that around found in the system, Then a duplicate Email validation error should display.",
                TestName = "Duplicate Email Validation",
                TestNumber = "UI_UpPro12"
            };

            using (var app = this.CreateSSOApp(testCaseDescription))
            {
                var differentUserEmail = new TestUser()
                {
                    Email = app.DataStateManager.RetrieveInputUserFromDBOtherThanThese(existingUser.Email).Email
                };

                app.UpdateProfileFeature.PostUpdateData(differentUserEmail, false);
                app.UpdateProfileFeature.AssertExpectedValidationsAreDisplaying(UpdateProfileFeature.UpdateProfileValidations.emailDuplicate);
            }
        }

        [TestMethod]
        public void UI_UpPro13_ConfirmEmailValidation()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = profileUpdateFamily,
                TestDescription =
                    "Given an existing user updates their profile, When the Confirm Email doesn't match their email,, Then validation errors should display.",
                TestName = "Confirm Email Validation",
                TestNumber = "UI_UpPro13"
            };

            using (var app = this.CreateSSOApp(testCaseDescription))
            {
                var temp = new TestUser() {Email = "DoesntMatter@WhoCares.com", ConfirmEmail = "Whatever@whocares.com", FirstName = "Mike"};
                app.UpdateProfileFeature.PostUpdateData(temp, false);
                
                app.UpdateProfileFeature.AssertExpectedValidationsAreDisplaying(UpdateProfileFeature.UpdateProfileValidations.confirmEmailDoesntMatch);
            }
        }

        private IdentifixSSOApp CreateSSOApp(TestCaseHeaderData header)
        {
            var app = new IdentifixSSOApp(header);

            //existingUser IS STATIC!
            //Create a single user for all of these tests 
            if (existingUser == null)
            {
                existingUser = app.DataStateManager.CreateRandomNewUserObject();
                app.Workflow.RegisterUser(existingUser, true, 60);
            }
            
            app.Workflow.Login(existingUser.Email, existingUser.Password);
            app.NavigationFeature.ToProfileUpdate();
            return app;
        }
    }
}
