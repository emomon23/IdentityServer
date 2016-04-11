using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Identifix.IdentityServer.Infrastructure;
using log4net;
using Autofac;

namespace Identifix.IdentityServer.Controllers
{
    public abstract class MvcControllerBase : Controller
    {
        protected MvcControllerBase(IApplicationContext context)
        {
            Guard.IsNotNull(context, "context");
            this.Context = context;
        }

        protected IApplicationContext Context { get; private set; }

        public IStateManager State => this.Context.State;

        public ISettingManager Settings => this.Context.Settings;

        public ILog Logger => this.Context.Logger;
    };
}