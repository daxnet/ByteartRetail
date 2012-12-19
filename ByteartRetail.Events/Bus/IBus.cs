using ByteartRetail.Events.Handlers;
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
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        bool IsDistributedTransactionSupported { get; }
    }
}
