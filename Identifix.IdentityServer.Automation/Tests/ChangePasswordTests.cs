using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
    public class ChangePasswordTests : BaseTestClass
    {
        private TestUser existingUser;
      
        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg50_ChangePwd_StrongPasswordValidation()
        {
            var testDesc = "Given a user is Changeing their Password for Identifix, When they enter a 'Weak' password, Then a validation error should display";

            using (var app = this.CreateAndInitializeSSOApp("UI_Reg50", "Chg Pwd - Strong Password Validation", testDesc))
            {
                app.ChgPwdFeature.ChangePassword(existingUser.Password, "password", false);
                app.ChgPwdFeature.AssertExpectedValidationsAreShowing(ChangePasswordFeature.ChgPwdValidationsEnumerations.invalidNewPassword);
           }

        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg51_ChangePwd_InvalidCurrentPassword()
        {
                var testDesc =
                  "Given a user is Changing their Password for Identifix, When they enter an incorrect 'Current Password' and click Submit, Then an error message should display";
                var testName = "Chg Pwd - Incorrect Current Password";
                var testNbr = "UI_Reg51";

            using (var app = this.CreateAndInitializeSSOApp(testNbr, testName, testDesc))
            {
                app.ChgPwdFeature.ChangePassword("An Invalid Password", "P@ssword234!");
                app.ChgPwdFeature.AssertExpectedValidationsAreShowing(ChangePasswordFeature.ChgPwdValidationsEnumerations.invalidCurrentPassword);
            }
        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg52_ChangePasswordSuccessfully()
        {
            var testDesc =
                "Given a user is changing their password for Identifix, When they provide the correct 'Current Password' and a valid 'New Password' and 'Confirm Password', Then their passsword should be changed ";
            var testName = "Chg Pwd - Successfully";
            var testNbr = "UI_Reg52";

            using (var app = this.CreateAndInitializeSSOApp(testNbr, testName, testDesc))
            {
                var currentPassword = existingUser.Password;
                var newPassword = "NewP@ssword7!";
                
                app.ChgPwdFeature.ChangePassword(currentPassword, newPassword);
                app.NavigationFeature.AssertCurrentPage(NavigationFeature.PageNameEnumerations.DataSavedPage);
            }
        }

        //[TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg53_NewPasswordIsSameAsOldPassword()
        {
            var testDesc = "Given a user is changing their password for Identifix, When their new password is the same as their old password, Then a validation error should display";
            var testName = "Chg Pwd - New Pwd same as old Pwd";
            var testNbr = "UI_Reg53";

            using (var app = this.CreateAndInitializeSSOApp(testNbr, testName, testDesc))
            {
                app.ChgPwdFeature.ChangePassword(existingUser.Password, existingUser.Password);
                app.ChgPwdFeature.AssertExpectedValidationsAreShowing(ChangePasswordFeature.ChgPwdValidationsEnumerations.newPasswordSameAsOld);
            }

        }

        [TestMethod]
        [TestCategory("UIAutomation")]
        public void UI_Reg54_ConfirmPasswordDoesntMatchNewPassword()
        {
            var desc = "Given an existing user is changing their password, When their confirm password doesn't match their new password, Then a validation error should dispaly";

            using (var app = this.CreateAndInitializeSSOApp("UI_Reg54", "Confirm Password Doesn't Match New Password", desc))
            {
                string newPwd = "NewP@ssword234!";
                string confirmPwd = "Not23TheSamePwd!";

                app.ChgPwdFeature.ChangePassword(existingUser.Password, newPwd, false, confirmPwd);
                app.ChgPwdFeature.AssertExpectedValidationsAreShowing(ChangePasswordFeature.ChgPwdValidationsEnumerations.passwordDontMatch);

                app.ChgPwdFeature.ChangePassword(existingUser.Password, newPwd, true, confirmPwd);
                app.ChgPwdFeature.AssertExpectedValidationsAreShowing(ChangePasswordFeature.ChgPwdValidationsEnumerations.passwordDontMatch);


            }

        }

        private IdentifixSSOApp CreateAndInitializeSSOApp(string testNbr, string testName, string testDesc)
        {
            TestCaseHeaderData header = new TestCaseHeaderData()
            {
                TestName = testName,
                TestDescription = testDesc,
                TestNumber = testNbr,
                TestFamily = "Change Password"
            };

            var app = new IdentifixSSOApp(header);
            existingUser = app.Workflow.NavigateToChangePasswordForAnExistingUser();

            return app;
        }
    }
}
