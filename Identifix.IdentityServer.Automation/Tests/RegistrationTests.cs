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
    public class RegistrationTests : BaseTestClass
    {
        private string registrationFamily = "Registration";
        private TestUser newUser;

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg10_UserRegistersSuccessfully()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = registrationFamily,
                TestDescription =
                    "Given a new user registers for the site, When they submit their data, Then a new record should be created and they should be able to log in",
                TestName = "UserRegistersSuccessfully",
                TestNumber = "UI_Reg10"
            };

            using (var app =this.CreateNewIdentifixSSOApp(testCaseDescription))
            {
                app.RegistrationFeature.PostNewUserData(newUser);

                app.AssertTrue(app.CurrentPage.Contains("confirmemail"), "User should have been registered, and redirected to the Check Email page", true);
                
                app.LoginUser(newUser);
                app.AssertTrue(app.CurrentPage.Contains("about"), "Should be redirected to about page", true);
            }
        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg20_UserRegistersDuplicateEmailErrorValidation_WithoutPostingForm()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = "Registration",
                TestDescription =
                    "Given a new user registers for the site, and they enter a email address already in our system, When they press a key, Then an email error message should display",
                TestName = "UserRegistration_DuplicateEmail_No_Posting_Data",
                TestNumber = "UI_Reg20"
            };

            using (var app = this.CreateNewIdentifixSSOApp(testCaseDescription))
            {
                var existingUser = app.DataStateManager.RetrieveInputUserFromDB();
                var newUser = new TestUser() {Email = existingUser.Email, FirstName = "ForceTextUpdate"};
                app.RegistrationFeature.PostNewUserData(newUser, false);

                app.RegistrationFeature.AssertValidationsAreShowingAsExpected(
                    RegistrationFeature.RegistrationValidations.emailDuplicate);
            }
        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg12_ConfirmEmailValidation()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = registrationFamily ,
                TestDescription =
                   "Given a user is registering for Identifix, When they provide a confirmation email that doesn't match their email, Then a validation error should immedately dispaly",
                TestName = "Confirm Email Validation",
                TestNumber = "UI_Reg12"
            };

            using (var app = this.CreateNewIdentifixSSOApp(testCaseDescription))
            {
                newUser.ConfirmEmail = "NotTheSameAsEmail@email.com";
                app.RegistrationFeature.PostNewUserData(newUser, false, false);

                app.RegistrationFeature.AssertValidationsAreShowingAsExpected(RegistrationFeature.RegistrationValidations.confirmEmailDoesntMatch);
            }
        }

        //[TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg14_Registration_ConfirmPasswordMismatch()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = registrationFamily,
                TestDescription =
                   "Given a user is registering for Identifix, When their confirm password doesn't match their password, Then a validation error should display",
                TestName = "Confirm Password Mismatch",
                TestNumber = "UI_Reg14"
            };

            using (var app = this.CreateNewIdentifixSSOApp(testCaseDescription))
            {
                newUser.ConfirmPassword = "NotTheSameAsPassword";
                app.RegistrationFeature.PostNewUserData(newUser, false, false);

                app.RegistrationFeature.AssertValidationsAreShowingAsExpected(RegistrationFeature.RegistrationValidations.confirmPasswordDoesntMatch);
            }
        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg16_Registration_StrongPasswordValiation()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = registrationFamily,
                TestDescription =
                  "Given a user is registering for Identifix, When the user enters a 'Weak' password, Then a validation error should display",
                TestName = "Strong Password Valdiation",
                TestNumber = "UI_Reg16"
            };

            using (var app = this.CreateNewIdentifixSSOApp(testCaseDescription))
            {
                newUser.Password = "weak";
                newUser.ConfirmPassword = newUser.Password;

                app.RegistrationFeature.PostNewUserData(newUser, true);
                app.RegistrationFeature.AssertValidationsAreShowingAsExpected(RegistrationFeature.RegistrationValidations.invalidPassword);
            }
        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg30_MissingRequiredInputValidations()
        {
            TestCaseHeaderData testCaseDescription = new TestCaseHeaderData()
            {
                TestFamily = "Registration",
                TestDescription =
                    "Given a user goes to register, and they have provided no data, When they click the save button, then all the validation should display",
                TestName = "Missing Required Input Validation",
                TestNumber = "UI_Reg30"
            };

            using (var app = new IdentifixSSOApp(testCaseDescription))
            {
                var newUser = new TestUser();
                app.Workflow.RegisterUser(newUser, true, 1);

                var validationResult =
                    app.RegistrationFeature.AreExpectedValidationAreShowing(
                        RegistrationFeature.RegistrationValidations.emailRequired,
                        RegistrationFeature.RegistrationValidations.passwordRequired,
                        RegistrationFeature.RegistrationValidations.confirmEmailRequired,
                        RegistrationFeature.RegistrationValidations.firstNameRequired,
                        RegistrationFeature.RegistrationValidations.lastNameRequired,
                        RegistrationFeature.RegistrationValidations.address1Required,
                        RegistrationFeature.RegistrationValidations.cityRequired,
                        RegistrationFeature.RegistrationValidations.zipRequired,
                        RegistrationFeature.RegistrationValidations.passwordRequired,
                        RegistrationFeature.RegistrationValidations.confirmPasswordRequired);

                app.AssertTrue(validationResult.AssertionOfValidationsDisplayingPassed, validationResult.AssertionFailureMessage);
            }
        }

        private IdentifixSSOApp CreateNewIdentifixSSOApp(TestCaseHeaderData header)
        {
            var app = new IdentifixSSOApp(header);
            app.TouchSecureResourceRedirects("About");
            app.LoginFeature.AssertAmOnLoginPage();
            app.NavigationFeature.ToUserRegistration();

            var dataStateManager = app.DataStateManager;
            newUser = dataStateManager.CreateRandomNewUserObject();

            return app;
        }
    }
}
