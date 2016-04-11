using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identifix.IdentityServer.Tests
{
    [TestClass]
    public class GuardExceptionTests
    {
        // SetUp & Mocks
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Instantiation_DefaultConstructor_InitializesMessage()
        {
            GuardException sut = new GuardException();

            Assert.IsNotNull(sut.Message);
            Assert.AreEqual(GuardException.DefaultMessage, sut.Message);
        }

        [TestMethod]
        public void Instantiation_MessageOnlyConstructor_InitializesMessage()
        {
            const string message = "Exceptional Message";
            GuardException sut = new GuardException(message);

            Assert.IsNotNull(sut.Message);
            Assert.AreEqual(message, sut.Message);
        }

        [TestMethod]
        public void Instantiation_InnerExceptionConstructor_InitializesInnerException()
        {
            const string message = "Exceptional Message";
            Exception inner = new Exception();
            GuardException sut = new GuardException(message, inner);

            Assert.IsNotNull(sut.InnerException);
            Assert.AreSame(inner, sut.InnerException);
        }

        [TestMethod]
        public void Instantiation_InnerExceptionConstructor_InitializesMessage()
        {
            const string message = "Exceptional Message";
            Exception inner = new Exception();
            GuardException sut = new GuardException(message, inner);

            Assert.IsNotNull(sut.Message);
            Assert.AreSame(message, sut.Message);
        }

        [TestMethod]
        public void Instantiation_WithSerializationInfo_ReinstantiatesException()
        {
            string uniqueMessage = Guid.NewGuid().ToString();
            string helpUrl = $"http://www.url.com/{Guid.NewGuid()}";
            SerializationInfo info = new SerializationInfo(typeof(GuardException), new FormatterConverter());
            info.AddValue("ClassName", typeof(GuardException).FullName);
            info.AddValue("Message", uniqueMessage);
            info.AddValue("InnerException", null);
            info.AddValue("HelpURL", helpUrl);
            info.AddValue("StackTraceString", string.Empty);
            info.AddValue("RemoteStackTraceString", string.Empty);
            info.AddValue("RemoteStackIndex", 0);
            info.AddValue("ExceptionMethod", string.Empty);
            info.AddValue("HResult", 1);
            info.AddValue("Source", string.Empty);

            GuardException sut = new GuardException(info, new StreamingContext());

            Assert.AreEqual(uniqueMessage, sut.Message);
            Assert.AreEqual(helpUrl, sut.HelpLink);
        }
    }
}