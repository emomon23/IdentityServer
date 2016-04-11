using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Identifix.IdentityServer.Infrastructure;
using Identifix.IdentityServer.Models;
using Identifix.IdentityServer.Models.Data;

namespace Identifix.IdentityServer.API
{
    public class ReferenceDataController : BaseApiController
    {
        private IUserService userService = null;

        [HttpGet]
        public APIResult GetCountries()
        {
            var countries = userService.GetCountries();
            return new APIResult()
            {
                IsSuccessful = true,
                Payload = countries
            };
        }

        [HttpGet]
        public APIResult GetStates()
        {
            var payload = userService.GetStates();

            return new APIResult() {IsSuccessful = true, Payload = payload};
        }

        public ReferenceDataController(IApplicationContext context, IUserService userService) : base(context)
        {
            this.userService = userService;
        }
        
    }
}
