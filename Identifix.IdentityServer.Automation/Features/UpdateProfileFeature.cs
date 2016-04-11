using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;
using iEmosoft.Automation.HelperObjects;
using Identifix.IdentityServer.Automation.DataState.Model;

namespace Identifix.IdentityServer.Automation.Features
{
    public class UpdateProfileFeature
    {
        public enum UpdateProfileValidations
        {
            emailDuplicate,
            emailInvalidFormat,
            emailRequired,
            confirmEmailRequired,
            confirmEmailInvalidFormat,
            confirmEmailDoesntMatch,
            firstNameRequired,
            lastNameRequired,
            shopNameRequired,
            address1Required,
            countryRequired,
            cityRequired,
            zipRequired
       }

        TestExecutioner executioner;

        public UpdateProfileFeature(TestExecutioner executioner)
        {
            this.executioner = executioner;
        }

        public void PostUpdateData(TestUser user, bool submitForm)
        {
            if (!user.Email.isNull())
            {
                executioner.SetTextOnAngualrWatchedElement("editUserEmail", user.Email);
            }

            if (!user.ConfirmEmail.isNull())
            {
                executioner.SetTextOnAngualrWatchedElement("editUserConfirmEmail", user.ConfirmEmail);
            }

            if (!user.FirstName.isNull())
            {
                executioner.SetTextOnElement("editUser_editUserFirstName", user.FirstName);

            }

            if (!user.LastName.isNull())
            {
                executioner.SetTextOnElement("editUser_editUserLastName", user.LastName);
            }

            if (!user.ShopName.isNull())
            {
                executioner.SetTextOnElement("editUserShopName", user.ShopName);
            }
            
            if (!user.Address1.isNull())
            {
                executioner.SetTextOnElement("editUserAddress1", user.Address1);
            }

            if (!user.Address2.isNull())
            {
                executioner.SetTextOnElement("editUserAddress2", user.Address2);
            }

            if (!user.City.isNull())
            {
                executioner.SetTextOnElement("editUserCity", user.City);
                
            }
            
            if (!user.ZipCode.isNull())
            {
                executioner.SetTextOnElement("editUserZipCode", user.ZipCode);
            }
            
            if (submitForm)
            {
                executioner.ClickElement("editUserSave", "", "", "Update the user profile form and click 'Save'",
                    "Your data should be saved", true, 5);
            }
        }

        public void ClearUpdateProfileForm(bool submitForm)
        {
            executioner.SetTextOnAngualrWatchedElement("editUserEmail", null);
            executioner.SetTextOnAngualrWatchedElement("editUserConfirmEmail", null);
            executioner.SetTextOnElement("editUser_editUserFirstName", null);
            executioner.SetTextOnElement("editUser_editUserLastName", null);
            executioner.SetTextOnElement("editUserShopName", null);
            executioner.SetTextOnElement("editUserAddress1", null);
            executioner.SetTextOnElement("editUser_editUserAddress2", null);
            executioner.SetTextOnAngualrWatchedElement("editUserCity", null);
            executioner.SetTextOnAngualrWatchedElement("editUserZipCode", null);

            if (submitForm)
            {
                executioner.ClickElement("editUserSave", "", "", "Update the user profile form and click 'Save'",
                    "Your data should be saved", true, 5);
            }
        }

        public void AssertExpectedValidationsAreDisplaying(params UpdateProfileValidations[] expectedValidations)
        {
            ValidationAssertionResult result = AreExpectedValidationsShowing(expectedValidations);
            if (!result.AssertionOfValidationsDisplayingPassed)
            {
                executioner.BeginTestCaseStep("Confirm validations are displaying as expected",
                    "Validations should be show");
                executioner.FailCurrentStep("", result.AssertionFailureMessage, true);
            }
        }

        public ValidationAssertionResult AreExpectedValidationsShowing(params UpdateProfileValidations[] expectedValidations)
        {
            ValidationAssertionResult result;
            string msg = "";

            foreach (var validation in expectedValidations)
            {
                //eg. editUser_emailRequired
                string spanId = "editUser_" + validation.ToString();
                if (!executioner.IsElementDisplaying(spanId))
                {
                    msg += spanId + ", ";
                }
            }

            if (msg != "")
            {
                msg = "Expected Validations are Missing: " + msg;
            }

            return new ValidationAssertionResult()
            {
                AssertionFailureMessage = msg,
                AssertionOfValidationsDisplayingPassed = msg == ""
            };

        }
    }
}
