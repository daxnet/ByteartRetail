using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.OrderDispatched
{
    public class UpdateStatusEventHandler : IEventHandler<OrderDispatchedEvent>
    {
        #region IEventHandler<OrderDispatchedEvent> Members

        public void Handle(OrderDispatchedEvent @event)
        {
            Utils.Log(string.Format("UpdateStatusEventHandler ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            @event.DispatchedOrder.DateDispatched = @event.DispatchedDate;
            @event.DispatchedOrder.Status = SalesOrderStatus.Dispatched;
        }

        #endregion
    }
}
