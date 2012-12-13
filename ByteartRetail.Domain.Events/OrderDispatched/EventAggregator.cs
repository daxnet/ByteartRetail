using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.OrderDispatched
{
    public class EventAggregator : EventAggregator<OrderDispatchedEvent>
    {
        public EventAggregator(EventDispatchMode dispatchMode, IEventHandler<OrderDispatchedEvent>[] eventHandlers)
            : base(dispatchMode, eventHandlers)
        { }
    }
}
