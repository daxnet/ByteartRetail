using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    public interface IBus
    {
        void Publish<TEvent>(TEvent evnt)
            where TEvent : class, IEvent;
        void Publish<TEvent>(IEnumerable<TEvent> evnts)
            where TEvent : class, IEvent;
        bool IsDistributedTransactionSupported { get; }
    }
}
