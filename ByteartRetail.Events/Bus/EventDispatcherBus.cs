using ByteartRetail.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteartRetail.Events.Bus
{
    public class EventDispatcherBus : IBus
    {
        #region Private Fields
        private readonly List<IEventDispatcher> eventDispatchers = new List<IEventDispatcher>();
        #endregion

        public EventDispatcherBus(IEventDispatcher[] eventDispatchers)
        {
            this.eventDispatchers.AddRange(eventDispatchers);
        }

        private IEventDispatcher GetEventDispatcher(Type eventType)
        {
            return this.eventDispatchers.Where(p =>
                {
                    Type eventDispatcherType = p.GetType();
                    if (eventDispatcherType.IsGenericType &&
                        eventDispatcherType.GetGenericArguments().Any(q => q == eventType))
                        return true;
                    return false;
                }).FirstOrDefault();
        }

        private IEventDispatcher<TEvent> GetEventDispatcher<TEvent>() where TEvent : class, IEvent
        {
            return GetEventDispatcher(typeof(TEvent)) as IEventDispatcher<TEvent>;
        }

        #region IBus Members

        public void Publish<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            var eventDispatcher = GetEventDispatcher(evnt.GetType());
            if (eventDispatcher != null)
                eventDispatcher.DispatchEvent(evnt);
        }

        public bool IsDistributedTransactionSupported
        {
            get { return false; }
        }

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : class, IEvent
        {
            IEventDispatcher<TEvent> eventDispatcher = GetEventDispatcher<TEvent>();
            if (eventDispatcher == null)
            {
                // 针对给定的事件类型，创建事件派发器。
                Type eventDispatcherType = typeof(EventDispatcher<>).MakeGenericType(typeof(TEvent));
                eventDispatcher = (IEventDispatcher<TEvent>)Activator.CreateInstance(eventDispatcherType, EventDispatchMode.Sequential, new IEventHandler<TEvent>[] { eventHandler });
                this.eventDispatchers.Add(eventDispatcher);
            }
            else
            {
                eventDispatcher.RegisterHandler(eventHandler);
            }
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : class, IEvent
        {
            IEventDispatcher<TEvent> eventDispatcher = GetEventDispatcher<TEvent>();
            if (eventDispatcher != null)
            {
                eventDispatcher.UnregisterHandler(eventHandler);
            }
        }

        #endregion
    }
}
