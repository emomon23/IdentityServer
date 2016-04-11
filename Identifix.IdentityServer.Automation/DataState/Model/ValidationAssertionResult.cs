using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identifix.IdentityServer.Automation.DataState.Model
{
    public class ValidationAssertionResult
    {
        public bool AssertionOfValidationsDisplayingPassed { get; set; }
        public string AssertionFailureMessage { get; set; }
    }
}
