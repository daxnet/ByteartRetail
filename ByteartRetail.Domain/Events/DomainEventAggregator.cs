using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示领域事件聚合器。
    /// </summary>
    internal sealed class DomainEventAggregator
    {
        private static readonly DomainEventAggregator instance = new DomainEventAggregator();
        private readonly object sync = new object();
        private readonly Dictionary<Type, List<object>> domainEventHandlers = new Dictionary<Type, List<object>>();

        static DomainEventAggregator() { }
        private DomainEventAggregator() { }

        public static DomainEventAggregator Instance
        {
            get { return instance; }
        }

        public void Subscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
            where TDomainEvent : class, IDomainEvent
        {
            lock (sync)
            {
                var eventType = typeof(TDomainEvent);
                if (domainEventHandlers.ContainsKey(eventType))
                {
                    var handlers = domainEventHandlers[eventType];
                    if (handlers != null)
                    {
                        if (!handlers.Exists(deh => deh.Equals(domainEventHandler)))
                            handlers.Add(domainEventHandler);
                    }
                    else
                    {
                        handlers = new List<object>();
                        handlers.Add(domainEventHandler);
                    }
                }
                else
                    domainEventHandlers.Add(eventType, new List<object> { domainEventHandler });
            }
        }

        public void Subscribe<TDomainEvent>(IEnumerable<IDomainEventHandler<TDomainEvent>> domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandler in domainEventHandlers)
                Subscribe<TDomainEvent>(domainEventHandler);
        }

        public void Subscribe<TDomainEvent>(params IDomainEventHandler<TDomainEvent>[] domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandler in domainEventHandlers)
                Subscribe<TDomainEvent>(domainEventHandler);
        }

        public void Subscribe<TDomainEvent>(Action<TDomainEvent> domainEventHandlerFunc)
            where TDomainEvent : class, IDomainEvent
        {
            Subscribe<TDomainEvent>(new ActionDelegatedDomainEventHandler<TDomainEvent>(domainEventHandlerFunc));
        }

        public void Subscribe<TDomainEvent>(IEnumerable<Func<TDomainEvent, bool>> domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandlerFunc in domainEventHandlerFuncs)
                Subscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public void Subscribe<TDomainEvent>(params Func<TDomainEvent, bool>[] domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandlerFunc in domainEventHandlerFuncs)
                Subscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public void Unsubscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
            where TDomainEvent : class, IDomainEvent
        {
            lock (sync)
            {
                var eventType = typeof(TDomainEvent);
                if (domainEventHandlers.ContainsKey(eventType))
                {
                    var handlers = domainEventHandlers[eventType];
                    if (handlers != null &&
                        handlers.Exists(deh => deh.Equals(domainEventHandler)))
                    {
                        var handlerToRemove = handlers.First(deh => deh.Equals(domainEventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }

        public void Unsubscribe<TDomainEvent>(IEnumerable<IDomainEventHandler<TDomainEvent>> domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandler in domainEventHandlers)
                Unsubscribe<TDomainEvent>(domainEventHandler);
        }

        public void Unsubscribe<TDomainEvent>(params IDomainEventHandler<TDomainEvent>[] domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandler in domainEventHandlers)
                Unsubscribe<TDomainEvent>(domainEventHandler);
        }

        public void Unsubscribe<TDomainEvent>(Action<TDomainEvent> domainEventHandlerFunc)
            where TDomainEvent : class, IDomainEvent
        {
            Unsubscribe<TDomainEvent>(new ActionDelegatedDomainEventHandler<TDomainEvent>(domainEventHandlerFunc));
        }

        public void Unsubscribe<TDomainEvent>(IEnumerable<Func<TDomainEvent, bool>> domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandlerFunc in domainEventHandlerFuncs)
                Unsubscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public void Unsubscribe<TDomainEvent>(params Func<TDomainEvent, bool>[] domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            foreach (var domainEventHandlerFunc in domainEventHandlerFuncs)
                Unsubscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public void UnsubscribeAll<TDomainEvent>()
            where TDomainEvent : class, IDomainEvent
        {
            lock (sync)
            {
                var eventType = typeof(TDomainEvent);
                if (domainEventHandlers.ContainsKey(eventType))
                {
                    var handlers = domainEventHandlers[eventType];
                    if (handlers != null)
                        handlers.Clear();
                }
            }
        }

        public void UnsubscribeAll()
        {
            lock (sync)
            {
                domainEventHandlers.Clear();
            }
        }

        public IEnumerable<IDomainEventHandler<TDomainEvent>> GetSubscriptions<TDomainEvent>()
            where TDomainEvent : class, IDomainEvent
        {
            var eventType = typeof(TDomainEvent);
            if (domainEventHandlers.ContainsKey(eventType))
            {
                var handlers = domainEventHandlers[eventType];
                if (handlers != null)
                    return handlers.Select(p => p as IDomainEventHandler<TDomainEvent>).ToList();
                else
                    return null;
            }
            else
                return null;
        }

        public void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            if (domainEvent == null)
                throw new ArgumentNullException("domainEvent");
            var eventType = domainEvent.GetType();
            if (domainEventHandlers.ContainsKey(eventType) &&
                domainEventHandlers[eventType] != null &&
                domainEventHandlers[eventType].Count > 0)
            {
                var handlers = domainEventHandlers[eventType];
                foreach (var handler in handlers)
                {
                    var domainEventHandler = handler as IDomainEventHandler<TDomainEvent>;
                    if (domainEventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                    {
                        Task.Factory.StartNew((o) => domainEventHandler.Handle((TDomainEvent)o), domainEvent);
                    }
                    else
                    {
                        domainEventHandler.Handle(domainEvent);
                    }
                }
            }
        }

        public void Publish<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            if (domainEvent == null)
                throw new ArgumentNullException("domainEvent");
            var eventType = domainEvent.GetType();
            if (domainEventHandlers.ContainsKey(eventType) &&
                domainEventHandlers[eventType] != null &&
                domainEventHandlers[eventType].Count > 0)
            {
                var handlers = domainEventHandlers[eventType];
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        var domainEventHandler = handler as IDomainEventHandler<TDomainEvent>;
                        if (domainEventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew((o) => domainEventHandler.Handle((TDomainEvent)o), domainEvent));
                        }
                        else
                        {
                            domainEventHandler.Handle(domainEvent);
                        }
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
                callback(domainEvent, false, null);
        }
    }
}
