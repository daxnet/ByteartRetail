using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public abstract class EventAggregator<TEvent> : IEventAggregator<TEvent>
        where TEvent : class, IDomainEvent
    {
        protected readonly List<IEventHandler<TEvent>> eventHandlers = new List<IEventHandler<TEvent>>();
        protected readonly EventDispatchMode dispatchMode;

        public EventAggregator(EventDispatchMode dispatchMode, IEventHandler<TEvent>[] eventHandlers)
        {
            this.dispatchMode = dispatchMode;
            foreach (var eh in eventHandlers)
                this.RegisterHandler(eh);
        }

        #region IEventAggregator<TEvent> Members

        public virtual void RegisterHandler(IEventHandler<TEvent> eventHandler)
        {
            if (!eventHandlers.Contains(eventHandler))
                eventHandlers.Add(eventHandler);
        }

        public virtual void DispatchEvent(TEvent domainEvent)
        {
            switch (dispatchMode)
            {
                case EventDispatchMode.Sequential:
                    foreach (var eventHandler in eventHandlers)
                        eventHandler.Handle(domainEvent);
                    break;
                case EventDispatchMode.Parallel:
                    Parallel.ForEach<IEventHandler<TEvent>>(eventHandlers,
                        p => p.Handle(domainEvent));
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region IEventAggregator Members

        public void RegisterHandler(IEventHandler<IDomainEvent> eventHandler)
        {
            IEventHandler<TEvent> genericEventHandler = eventHandler;
            this.RegisterHandler(genericEventHandler);
        }

        public EventDispatchMode DispatchMode
        {
            get { return this.dispatchMode; }
        }

        public void DispatchEvent(IDomainEvent domainEvent)
        {
            if (domainEvent is TEvent)
            {
                DispatchEvent(domainEvent as TEvent);
            }
        }
        #endregion
    }
}
