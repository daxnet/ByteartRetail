using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.Handlers
{
    public class ConfirmOrderDomainEventHandler : IDomainEventHandler<ConfirmOrderEvent>
    {
        public void Handle(ConfirmOrderEvent evnt)
        {
            SalesOrder salesOrder = evnt.Source as SalesOrder;
            salesOrder.DateDelivered = evnt.ConfirmedDate;
            salesOrder.Status = SalesOrderStatus.Delivered;
        }
    }
}
