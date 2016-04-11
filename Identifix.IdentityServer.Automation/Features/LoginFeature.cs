using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.QualityTools.UnitTestFramework;
using iEmosoft.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Automation.Features
{
    public class LoginFeature
    {
        TestExecutioner exectioner;

        public LoginFeature(TestExecutioner executioner)
        {
            this.exectioner = executioner;
        }

        public bool IsLoggedIn { get; private set; }

        public bool Login(string userName, string password)
        {
            try
            {
                exectioner.SetTextOnElement("username", userName);
                exectioner.SetTextOnElement("password", password);

                exectioner.ClickElement("authenticate.login", "", "", "Enter username and password, and click Login", "",
                    true, 10);


                JQuerySelector jQuerySelector = new JQuerySelector("Button:contains('Yes, Allow')");
                if (exectioner.DoesElementExist(jQuerySelector, 2))
                {
                    exectioner.ClickElement(jQuerySelector);
                }

                return exectioner.CurrentFormName_OrURL.Contains("identity/login?") == false;
            }
            catch (Exception exp)
            {
                throw exectioner.FailCurrentStep(exp);
            }
        }

        public void AssertAmOnLoginPage()
        {
            try
            {
                bool urlContainsLogin = exectioner.CurrentFormName_OrURL.ToLower().Contains("login?signin");
                if (!urlContainsLogin)
                {
                    exectioner.FailCurrentStep("Expected to be on login page",
                        "Am not on the login page (url: login?signin)", true);
                }
            }
            catch (Exception exp)
            {
                throw exectioner.FailCurrentStep(exp);
            }
        }
    }
}
