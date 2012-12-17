using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ByteartRetail.Application;
using ByteartRetail.Domain.Repositories.EntityFramework;
using ByteartRetail.Domain.Events;
using ByteartRetail.Infrastructure;
using ByteartRetail.Events.Bus;

namespace ByteartRetail.Services
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ByteartRetailDbContextInitailizer.Initialize();
            ApplicationService.Initialize();
            EventAggregatorBus.RegisterAggregator<OrderDispatchedEvent>();
            var bus = ServiceLocator.Instance.GetService<IBus>();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}