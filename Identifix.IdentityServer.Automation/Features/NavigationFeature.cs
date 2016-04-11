using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;

namespace Identifix.IdentityServer.Automation.Features
{
    public class NavigationFeature
    {
        public enum PageNameEnumerations
        {
            LoginPage,
            RegistrationPage,
            ChangePasswordPage,
            ResetPasswordPage,
            ProfileUpatePage,
            MockDirectHitLandingPage,
            SecurePage,
            CheckEmailPage,
            DataSavedPage,
            Unknown
        }

        TestExecutioner executioner;

        public NavigationFeature(TestExecutioner executioner)
        {
            this.executioner = executioner;
        }

        public bool ToUserRegistration()
        {
            try
            {
                executioner.ClickElement("authenticate.createAccount", "", "", "Click the Create Account link",
                    "Should navigate to the User Registration view", true, 10);

                return executioner.DoesElementExist("userRegistrationForm", 3);
            }
            catch (Exception exp)
            {
                throw executioner.FailCurrentStep(exp);
            }
        }

        public bool ToProfileUpdate()
        {
            try
            {
                executioner.ClickElement("href", "ChangePassword", "a", "Click the Profile link at the top of the page",
                    "Should be taken to the Profile page", true, 20);

                return executioner.CurrentFormName_OrURL.Contains("/update?uid");
            }
            catch (Exception exp)
            {
                throw executioner.FailCurrentStep(exp);
            }
        }

        public bool ToChangePassword()
        {
            try
            {
                executioner.ClickElement("editUserProfileChangePassword", "", "", "Click the Change Password Link",
                    "Nav to Change Pwd", true, 5);

                return executioner.CurrentFormName_OrURL.Contains("changePassword?uid");
            }
            catch (Exception exp)
            {
                throw executioner.FailCurrentStep(exp);
            }
        }

        public string HomePageURL
        {
            get { return ConfigurationManager.AppSettings["SecureClientSite"]; }

        }

        public void ToHomePage()
        {
            executioner.NavigateTo(this.HomePageURL);
        }

        public void AssertCurrentPage(PageNameEnumerations expectedPage)
        {
            if (expectedPage != CurrentPage)
            {
                executioner.FailCurrentStep("Expected to be at " + expectedPage.ToString(), CurrentPage.ToString(), true);
            }
        }

        public PageNameEnumerations CurrentPage
        {
            get
            {
                string currentUrl = executioner.CurrentFormName_OrURL;
                string mockPage = this.HomePageURL;

                if (currentUrl.Contains("login?"))
                {
                    return PageNameEnumerations.LoginPage;
                }

                if (currentUrl.Contains("#/update"))
                {
                    return PageNameEnumerations.ProfileUpatePage;
                }

                if (currentUrl == mockPage)
                {
                    return PageNameEnumerations.MockDirectHitLandingPage;
                }
                
                if (currentUrl.Contains("#/changePassword"))
                {
                    return PageNameEnumerations.ChangePasswordPage;
                }

                if (currentUrl.Contains("#/saved"))
                {
                    return PageNameEnumerations.DataSavedPage;
                }

                if (currentUrl.Contains("#/confirmEmail"))
                {
                    return PageNameEnumerations.CheckEmailPage;
                }

                JQuerySelector selector = new JQuerySelector("$('h1:contains(\"User Registration\")')");
                if (executioner.DoesElementExist(selector))
                {
                    return PageNameEnumerations.RegistrationPage;
                }

                return PageNameEnumerations.Unknown;
            }
        }
    }
}
