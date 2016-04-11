using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identifix.IdentityServer.Models
{
    public class APIResult
    {
        public bool IsSuccessful { get; set; }
        public object Payload { get; set; }
        public string ErrorMessage { get; set; }
    }

    public static class APIResultFactory
    {

        public static APIResult CreateAPIResult(Exception exp, string errorMessageOverride = "", object payload = null)
        {
            //Log this exception
            var result = new APIResult() { ErrorMessage = string.IsNullOrEmpty(errorMessageOverride) ? exp.Message : errorMessageOverride, IsSuccessful = false };
            if (payload != null)
            {
                result.Payload = payload;
            }

            return result;
        }

        public static APIResult CreateAPIResult(bool isSuccess, string notSuccessMessage = "", object payload = null)
        {
            //Log this is not successfull
            APIResult result = new APIResult()
            {
                IsSuccessful = isSuccess,
                ErrorMessage = isSuccess ? "" : notSuccessMessage,
                Payload = payload == null ? null : payload
            };

            return result;
        }

    }
}