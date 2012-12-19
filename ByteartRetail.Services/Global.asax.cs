using ByteartRetail.Application;
using ByteartRetail.Domain.Events;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.EntityFramework;
using ByteartRetail.Events.Bus;
using ByteartRetail.Infrastructure;
using System;

namespace ByteartRetail.Services
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ByteartRetailDbContextInitailizer.Initialize();
            ApplicationService.Initialize();
            DomainEventAggregator.Subscribe<OrderDispatchedEvent>(p =>
                {
                    SalesOrder salesOrder = p.Source as SalesOrder;
                    salesOrder.DateDispatched = p.DispatchedDate;
                    salesOrder.Status = SalesOrderStatus.Dispatched;
                });
            DomainEventAggregator.Subscribe<OrderConfirmedEvent>(p =>
                {
                    SalesOrder salesOrder = p.Source as SalesOrder;
                    salesOrder.DateDelivered = p.ConfirmedDate;
                    salesOrder.Status = SalesOrderStatus.Delivered;
                });
            IBus bus = ServiceLocator.Instance.GetService<IBus>();
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