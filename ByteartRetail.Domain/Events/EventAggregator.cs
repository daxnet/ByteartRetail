using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public static class EventAggregator
    {
        private static readonly Dictionary<Type, List<Delegate>> domainEventHandlers = new Dictionary<Type, List<Delegate>>();

        public static void Publish<TEvent>(TEvent evnt)
            where TEvent : class, IDomainEvent
        {
            if (domainEventHandlers.ContainsKey(typeof(TEvent)))
            {
                var delegates = domainEventHandlers[typeof(TEvent)];
                foreach (var del in delegates)
                {
                    Action<TEvent> action = (Action<TEvent>)Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(typeof(TEvent)), del.Method);
                    action(evnt);
                }
            }
        }

        public static void Subscribe<TEvent>(IDomainEventHandler<TEvent> domainEventHandler)
            where TEvent : class, IDomainEvent
        {
            Subscribe<TEvent>(p => domainEventHandler.Handle(p));
        }

        public static void Subscribe<TEvent>(Action<TEvent> domainEventHandlerAction)
            where TEvent : class, IDomainEvent
        {
            
        }
    }
}
