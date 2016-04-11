using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;
using iEmosoft.Automation.Test.IEmosoft.com;
using Identifix.IdentityServer.Automation.DataState.Model;

namespace Identifix.IdentityServer.Automation.Features
{
    public class ChangePasswordFeature
    {
        public enum ChgPwdValidationsEnumerations
        {
            invalidNewPassword,
            passwordRequired,
            newPasswordRequired,
            weakConfirmPassword,
            passwordDontMatch,
            invalidCurrentPassword,
            newPasswordSameAsOld
        }


        UIQuery currentPassword = new UIQuery("currentPassword");

        TestExecutioner executioner;

        public ChangePasswordFeature(TestExecutioner executioner)
        {
            this.executioner = executioner;
        }

        public void AssertExpectedValidationsAreShowing(params ChgPwdValidationsEnumerations[] expectedValudations)
        {
            var result = AreExpectedValidationAreShowing(expectedValudations);
            if (!result.AssertionOfValidationsDisplayingPassed)
            {
                executioner.FailCurrentStep("Validation should be showing", result.AssertionFailureMessage, true);
            }
        }
        public ValidationAssertionResult AreExpectedValidationAreShowing(params ChgPwdValidationsEnumerations[] expectedValidations)
        {
            try
            {
                string validationsNotShowing = "The following validations are not displaying as expected: ";
                bool assertionIsGood = true;

                //DoesElementExist will wait 2 seconds for the 1st validation to show,
                //Then it will go to 0;
                int checkForSeconds = 2;

                foreach (ChgPwdValidationsEnumerations validation in expectedValidations)
                {
                    string validationId = validation == ChgPwdValidationsEnumerations.invalidCurrentPassword || validation == ChgPwdValidationsEnumerations.newPasswordSameAsOld ? "errorMsg" : validation.ToString();

                    if (!executioner.IsElementDisplaying(validationId, checkForSeconds))
                    {
                        validationsNotShowing += validationId + ", ";
                        assertionIsGood = false;
                    }

                    checkForSeconds = 0;
                }

                return new ValidationAssertionResult()
                {
                    AssertionOfValidationsDisplayingPassed = assertionIsGood,
                    AssertionFailureMessage = assertionIsGood ? "" : validationsNotShowing
                };
            }
            catch (Exception exp)
            {
               throw executioner.FailCurrentStep(exp);
            }
        }

        public void ChangePassword(string currentPwd, string newPwd, bool submitForm = true, string confirmPassword = "SAME AS NEW")
        {
            var currentPasswordTextBox = "currentPassword";
            var newPasswordTextBox = "newPassword";
            var confirmPasswordTextBox = "confirmNewPassword";

            try
            {
                if (confirmPassword == "SAME AS NEW")
                {
                    confirmPassword = newPwd;
                }
                
                executioner.SetTextOnElement(currentPasswordTextBox, currentPwd);
                executioner.FireChangeEvent(currentPasswordTextBox);

                executioner.SetTextOnElement(newPasswordTextBox, newPwd);
                executioner.FireChangeEvent(newPasswordTextBox);

                executioner.SetTextOnElement(confirmPasswordTextBox, confirmPassword);
                executioner.FireChangeEvent(confirmPasswordTextBox);



                if (submitForm)
                {
                    executioner.ClickElement("changePassword", "", "",
                        "Enter your current password and your new pasword, and click 'Save'",
                        "Password should be change", true, 10);

                    //Refactor this when the spinner 'Talking to Server' is implemeneted!
                    executioner.Pause(2000);
                }
                else
                {
                    //We need to force focus away from the confirmPassword text bos
                    var script = "$('#changePassword').focus()";
                    executioner.ExecuteJavaScript(script);
                }
            }
            catch (Exception exp)
            {
               throw executioner.FailCurrentStep(exp);
            }
        }
    }
}
