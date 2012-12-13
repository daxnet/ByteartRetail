using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public class OrderDispatchedEvent : DomainEvent
    {
        public DateTime DispatchedDate { get; set; }
        public SalesOrder DispatchedOrder { get; set; }
    }
}
