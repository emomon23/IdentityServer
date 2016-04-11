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
    public class IdentifixSSOApp : IDisposable
    {
        TestExecutioner executioner = null;
        private LoginFeature loginFeature;
        private NavigationFeature navFeature;
        private RegistrationFeature regFeature;
        private AuthenticationDataState stateMachine;
        private Workflow workflow;
        private ChangePasswordFeature chgPwdFeature;
        private UpdateProfileFeature updateProfileFeature;

        private string secureClientURL = "";

        public IdentifixSSOApp()
        {
            executioner = new TestExecutioner();
        }

        public IdentifixSSOApp(TestCaseHeaderData header)
        {
            try
            {
                executioner = new TestExecutioner(header);
                this.NavigationFeature.ToHomePage();
            }
            catch (Exception exp)
            {
                throw executioner.FailTest(exp);
            }
        }

        public string DefaultTestFamily { get; set; }
        public void LaunchNewTest(string testNumber, string testName, string testDesc, string testFamily = "")
        {
            if (testFamily == "")
            {
                testFamily = this.DefaultTestFamily;
            }

            TestCaseHeaderData header = new TestCaseHeaderData()
            {
                TestFamily = testFamily,
                TestDescription = testDesc,
                TestName = testName,
                TestNumber = testNumber
            };

            this.executioner = new TestExecutioner(header);
            this.executioner.NavigateTo(this.NavigationFeature.HomePageURL);

        }
        
        public void Pause(int milliseconds)
        {
            executioner.Pause(milliseconds);
        }

        public UpdateProfileFeature UpdateProfileFeature
        {
            get
            {
                if (updateProfileFeature == null)
                {
                    updateProfileFeature = new UpdateProfileFeature(executioner);
                }

                return updateProfileFeature;
               
            }
        }
        public ChangePasswordFeature ChgPwdFeature
        {
            get
            {
                if (chgPwdFeature == null)
                {
                    chgPwdFeature = new ChangePasswordFeature(executioner);
                }

                return chgPwdFeature;
            }
        }
        public void LoginUser(TestUser user)
        {
            this.NavigationFeature.ToHomePage();
            this.TouchSecureResourceRedirects("About");

            this.LoginFeature.AssertAmOnLoginPage();
            this.loginFeature.Login(user.Email, user.Password);
      }

        public bool TouchSecureResourceRedirects(string href)
        {
            try
            {
                executioner.ClickElement("href", href, "a", "Click on the Secure link",
                    "Should be redirected to a log in page", true, 40);

                return executioner.CurrentFormName_OrURL.Contains(href) == false;
            }
            catch (Exception exp)
            {
                throw executioner.FailCurrentStep(exp);
            }
        }

        public Workflow Workflow
        {
            get
            {
                if (workflow == null)
                {
                    workflow = new Workflow(this, executioner);
                }

                return workflow;
                
            }
        }
        public void AssertTrue(bool shouldBeTrue, string because, bool isAShowStopper = true)
        {
            if (!shouldBeTrue)
            {
                executioner.FailCurrentStep("", because);
                if (isAShowStopper)
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(false, because);
                }
            }
        }

        public string CurrentPage
        {
            get { return executioner.CurrentFormName_OrURL.ToLower(); }
        }

        public AuthenticationDataState DataStateManager
        {
            get
            {
                if (stateMachine == null)
                {
                    stateMachine = new AuthenticationDataState(executioner);
                }
                return stateMachine;
            }
        }

        public LoginFeature LoginFeature
        {
            get
            {
                if (loginFeature == null)
                {
                    loginFeature = new LoginFeature(executioner);
                }

                return loginFeature;
               
            }
        }

        public RegistrationFeature RegistrationFeature
        {
            get
            {
                if (regFeature == null)
                {
                    regFeature = new RegistrationFeature(executioner);
                }

                return regFeature;
            }
        }

        public NavigationFeature NavigationFeature
        {
            get
            {
                if (navFeature == null)
                {
                    navFeature = new NavigationFeature(executioner);
                }

                return navFeature;
                ;
            }
        }

        public void Dispose()
        {
            executioner.Dispose();
        }
    }
}
