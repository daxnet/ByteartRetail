using ByteartRetail.Domain.Events;
using ByteartRetail.Domain.Model;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Handlers
{
    public class SendEmailHandler : IEventHandler<OrderDispatchedEvent>, IEventHandler<OrderConfirmedEvent>
    {
        #region IEventHandler<OrderDispatchedEvent> Members

        public void Handle(OrderDispatchedEvent evnt)
        {
            try
            {
                SalesOrder salesOrder = evnt.Source as SalesOrder;
                // 此处仅为演示，所以邮件内容很简单。可以根据自己的实际情况做一些复杂的邮件功能，比如
                // 使用邮件模板或者邮件风格等。
                Utils.SendEmail(salesOrder.User.Email,
                    "您的订单已经发货",
                    string.Format("您的订单 {0} 已于 {1} 发货。有关订单的更多信息，请与系统管理员联系。",
                    salesOrder.ID.ToString().ToUpper(), evnt.DispatchedDate));
            }
            catch (Exception ex)
            {
                // 如遇异常，直接记Log
                Utils.Log(ex);
            }
        }

        #endregion

        #region IEventHandler<OrderConfirmedEvent> Members

        public void Handle(OrderConfirmedEvent evnt)
        {
            try
            {
                SalesOrder salesOrder = evnt.Source as SalesOrder;
                // 此处仅为演示，所以邮件内容很简单。可以根据自己的实际情况做一些复杂的邮件功能，比如
                // 使用邮件模板或者邮件风格等。
                Utils.SendEmail(salesOrder.User.Email,
                    "您的订单已经确认收货",
                    string.Format("您的订单 {0} 已于 {1} 确认收货。有关订单的更多信息，请与系统管理员联系。",
                    salesOrder.ID.ToString().ToUpper(), evnt.ConfirmedDate));
            }
            catch (Exception ex)
            {
                // 如遇异常，直接记Log
                Utils.Log(ex);
            }
        }

        #endregion
    }
}
