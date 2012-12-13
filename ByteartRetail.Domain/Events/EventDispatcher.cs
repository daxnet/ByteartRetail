using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public static class EventDispatcher
    {
        private static readonly Dictionary<Type, IEventAggregator> eventAggregators = new Dictionary<Type, IEventAggregator>();

        public static void RegisterAggregator<TEvent>()
            where TEvent : class, IDomainEvent
        {
            if (eventAggregators.ContainsKey(typeof(TEvent)))
                return;

            IEventAggregator<TEvent> eventAggregator = ServiceLocator.Instance.GetService<IEventAggregator<TEvent>>();
            eventAggregators.Add(typeof(TEvent), eventAggregator);
        }

        public static void DispatchEvent<TEvent>(TEvent @event)
            where TEvent : class, IDomainEvent
        {
            var eventAggregator = eventAggregators[typeof(TEvent)];
            if (eventAggregator != null)
                eventAggregator.DispatchEvent(@event);
        }
    }
}
