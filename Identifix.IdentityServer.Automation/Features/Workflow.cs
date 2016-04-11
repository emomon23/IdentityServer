using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;
using iEmosoft.Automation.Model;
using iEmosoft.Automation.HelperObjects;
using Identifix.IdentityServer.Automation.DataState;
using Identifix.IdentityServer.Automation.DataState.Model;

namespace Identifix.IdentityServer.Automation.Features
{
    public class Workflow
    {
        private IdentifixSSOApp app;
        private TestExecutioner executioner;

        public Workflow(IdentifixSSOApp app, TestExecutioner executioner)
        {
            this.executioner = executioner;
            this.app = app;
        }

        public void NavigateToRegistrationPage()
        {
            if (app.NavigationFeature.CurrentPage == NavigationFeature.PageNameEnumerations.MockDirectHitLandingPage)
            {
                app.TouchSecureResourceRedirects("About");
                app.LoginFeature.AssertAmOnLoginPage();
            }

            app.NavigationFeature.ToUserRegistration();
        }

        public bool Login(string userName, string password)
        {
            app.NavigationFeature.ToHomePage();
            app.TouchSecureResourceRedirects("About");
            app.LoginFeature.Login(userName, password);

            return app.LoginFeature.IsLoggedIn;
        }

        public TestUser NavigateToChangePasswordForAnExistingUser()
        {
            //Register a new user
            var newUser = app.DataStateManager.CreateRandomNewUserObject();
            app.NavigationFeature.ToHomePage();
            app.TouchSecureResourceRedirects("About");
            RegisterUser(newUser, true, 60);

            //Login the new user
            app.NavigationFeature.ToHomePage();
            app.TouchSecureResourceRedirects("About");
            app.LoginFeature.Login(newUser.Email, newUser.Password);

            //Navigate to the change password UI
            app.NavigationFeature.ToProfileUpdate();
            app.NavigationFeature.ToChangePassword();

            return newUser;
        }

        public void RegisterUser(TestUser newUser, bool submitForm, int waitForURLToChange = 30)
        {
            this.NavigateToRegistrationPage();
            app.RegistrationFeature.PostNewUserData(newUser, submitForm, true, waitForURLToChange);
        }
    }
}
