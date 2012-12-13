using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.OrderDispatched
{
    public class SendEmailEventHandler : IEventHandler<OrderDispatchedEvent>
    {
        #region IEventHandler<OrderDispatchedEvent> Members

        public void Handle(OrderDispatchedEvent @event)
        {
            try
            {
                Utils.SendEmail(@event.DispatchedOrder.User.Email,
                    "Your Order Has Been Dispatched.",
                    string.Format("Your Order {0} has been dispatched on {1}. For more information please contact system administrator. Thank you for your order.",
                    @event.DispatchedOrder.ID.ToString().ToUpper(), @event.DispatchedDate));
            }
            catch(Exception ex)
            {
                Utils.Log(ex);
            }
        }

        #endregion
    }
}
