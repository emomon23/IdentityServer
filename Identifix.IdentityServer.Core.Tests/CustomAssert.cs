using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    public static class CustomAssert
    {
        public static TException Throws<TException>(Action action) where TException : Exception
        {
            TException exception = null;
            try
            {
                action.Invoke();
            }
            catch (TException tex)
            {
                exception = tex;
            }
            Assert.IsNotNull(exception);
            return exception;
        }

        public static void DoesNotThrow(Action action)
        {
            Exception exception = null;
            try
            {
                action.Invoke();
            }
            catch (Exception tex)
            {
                exception = tex;
            }
            Assert.IsNull(exception);
        }
    }
}
