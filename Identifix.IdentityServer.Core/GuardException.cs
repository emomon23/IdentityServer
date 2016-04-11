using System;
using System.Runtime.Serialization;

namespace Identifix.IdentityServer
{
    [Serializable] 
    public class GuardException : Exception 
    { 
        internal const string DefaultMessage = "A guard codition has been violated."; 
 
        public GuardException(): base(DefaultMessage) 
        { 
        } 
 
        public GuardException(SerializationInfo info, StreamingContext context): base(info, context) 
        { 
        } 
 
        public GuardException(string message): base(message ?? string.Empty) 
        { 
        } 
 
        public GuardException(string message, Exception innerException): base(message, innerException) 
        { 
        } 
    }

}