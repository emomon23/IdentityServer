using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;
using iEmosoft.Automation.HelperObjects;
using Identifix.IdentityServer.Automation.DataState.Model;

namespace Identifix.IdentityServer.Automation.Features
{
     public class RegistrationFeature
     {
         public enum RegistrationValidations
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
            zipRequired,
            passwordRequired,
            confirmPasswordRequired,
            confirmPasswordDoesntMatch,
            invalidPassword
        }

         TestExecutioner executioner;

         public RegistrationFeature(TestExecutioner executioner)
         {
             this.executioner = executioner;
         }

         public void AssertValidationsAreShowingAsExpected(params RegistrationValidations[] validations)
         {
            var result = AreExpectedValidationAreShowing(validations);
             if (!result.AssertionOfValidationsDisplayingPassed)
             {
                executioner.FailCurrentStep("", "Validations not showing: " + result.AssertionFailureMessage, true);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(result.AssertionOfValidationsDisplayingPassed, result.AssertionFailureMessage);
            }
           
         }

         public ValidationAssertionResult AreExpectedValidationAreShowing(params RegistrationValidations[] validationElements)
         {
             try
             {
                 string validationsNotShowing = "The following validations are not displaying as expected: ";
                 bool assertionIsGood = true;

                 //DoesElementExist will wait 2 seconds for the 1st validation to show,
                 //Then it will go to 0;
                 int checkForSeconds = 2;

                 foreach (RegistrationValidations validation in validationElements)
                 {
                     string validationId = validation.ToString();

                     //Selenium won't fire the digest cycle for the custom watch on dupliate email
                     //call the function directly
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

         public void PostNewUserData(TestUser inputUserData, bool submitForm = true, bool expectToSucceed = true, int waitForURLToChangeSeconds = 30)
         {
             try
             {
                 executioner.WaitForElementToVanish("countriesSpinner", 60);

                 if (!inputUserData.Email.isNull())
                 {
                     executioner.SetTextOnAngualrWatchedElement("newUserEmail", inputUserData.Email);
                 }

                 if (!inputUserData.ConfirmEmail.isNull())
                 {
                     executioner.SetTextOnAngualrWatchedElement("newUserConfirmEmail", inputUserData.ConfirmEmail);
                 }

                 if (!inputUserData.ShopName.isNull())
                     executioner.SetTextOnElement("newUserShopName", inputUserData.ShopName);

                 if (!inputUserData.Address1.isNull())
                     executioner.SetTextOnElement("newUserAddress1", inputUserData.Address1);

                 if (!inputUserData.FirstName.isNull())
                     executioner.SetTextOnElement("newUserFirstName", inputUserData.FirstName);

                 if (!inputUserData.LastName.isNull())
                     executioner.SetTextOnElement("newUserLastName", inputUserData.LastName);

                 if (!inputUserData.Address2.isNull())
                     executioner.SetTextOnElement("newUserAddress2", inputUserData.Address2);

                 if (!inputUserData.City.isNull())
                     executioner.SetTextOnElement("newUserCity", inputUserData.City);

                 if (!inputUserData.ZipCode.isNull())
                     executioner.SetTextOnElement("newUserZipCode", inputUserData.ZipCode);

                 if (!inputUserData.Password.isNull())
                 {
                     executioner.SetTextOnAngualrWatchedElement("newUserPassword", inputUserData.Password);
                     executioner.FireChangeEvent("newUserPassword");
                }
                 
                if (inputUserData.StateId > 0)
                     executioner.SetValueOnDropdown("newUserStateId", inputUserData.StateId.ToString());

                if (!inputUserData.ConfirmPassword.isNull())
                {
                    executioner.SetTextOnAngualrWatchedElement("newUserConfirmPassword", inputUserData.ConfirmPassword);
                }


                executioner.BeginTestCaseStep("Submit registration data", "", "See Image", true);

                 if (submitForm)
                 {
                     executioner.WaitForElementToVanish("countriesSpinner");

                     string expectedResult = expectToSucceed
                         ? "Check Email screen should display"
                         : "Validation Errors Should Display";

                     executioner.ClickElement("newUserSave", "", "", "Enter a new Registered User and click Save button",
                         expectedResult, true, waitForURLToChangeSeconds);
                 }

             }
             catch (Exception exp)
             {
                 throw executioner.FailCurrentStep(exp);
             }
         }
     }
}
