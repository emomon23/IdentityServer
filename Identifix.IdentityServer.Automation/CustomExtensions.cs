using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iEmosoft.Automation;
using OpenQA.Selenium;

namespace Identifix.IdentityServer.Automation
{
    public static  class CustomExtensions
    {
        public static void SetTextOnAngualrWatchedElement(this TestExecutioner e, string elementId, string value)
        {
            e.SetTextOnElement(elementId, null);
            e.Pause(200);
            e.SetTextOnElement(elementId, value + Keys.Tab);
        }
    }
}
