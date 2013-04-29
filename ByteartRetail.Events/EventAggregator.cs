using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly object sync = new object();
        private readonly Dictionary<Type, List<object>> eventHandlers = new Dictionary<Type, List<object>>();
        private readonly MethodInfo registerEventHandlerMethod;
        private readonly Func<object, object, bool> eventHandlerEquals = (o1, o2) =>
            {
                var o1Type = o1.GetType();
                var o2Type = o2.GetType();
                if (o1Type.IsGenericType &&
                    o1Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>) &&
                    o2Type.IsGenericType &&
                    o2Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>))
                    return o1.Equals(o2);
                return o1Type == o2Type;
            }; // checks if the two event handlers are equal. if the event handler is an action-delegated, just simply
        // compare the two with the object.Equals override (since it was overriden by comparing the two delegates. Otherwise,
        // the type of the event handler will be used because we don't need to register the same type of the event handler
        // more than once for each specific event.

        public EventAggregator()
        {
            registerEventHandlerMethod = (from p in this.GetType().GetMethods()
                                          let methodName = p.Name
                                          let parameters = p.GetParameters()
                                          where methodName == "Subscribe" &&
                                          parameters != null &&
                                          parameters.Length == 1 &&
                                          parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                                          select p).First();
        }

        public EventAggregator(object[] handlers)
            : this()
        {
            foreach (var obj in handlers)
            {
                var type = obj.GetType();
                var implementedInterfaces = type.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType &&
                        implementedInterface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventType = implementedInterface.GetGenericArguments().First();
                        var method = registerEventHandlerMethod.MakeGenericMethod(eventType);
                        method.Invoke(this, new object[] { obj });
                    }
                }
            }
        }

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (sync)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null)
                    {
                        if (!handlers.Exists(deh => eventHandlerEquals(deh, eventHandler)))
                            handlers.Add(eventHandler);
                    }
                    else
                    {
                        handlers = new List<object>();
                        handlers.Add(eventHandler);
                    }
                }
                else
                    eventHandlers.Add(eventType, new List<object> { eventHandler });
            }
        }

        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }

        public void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Subscribe<TEvent>(eventHandler);
        }

        public void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Subscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Subscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Subscribe<TEvent>(eventHandlerFunc);
        }

        public void Subscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Subscribe<TEvent>(eventHandlerFunc);
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            lock (sync)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null &&
                        handlers.Exists(deh => eventHandlerEquals(deh, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(deh => eventHandlerEquals(deh, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }

        public void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }

        public void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            foreach (var eventHandler in eventHandlers)
                Unsubscribe<TEvent>(eventHandler);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            Unsubscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Unsubscribe<TEvent>(IEnumerable<Func<TEvent, bool>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Unsubscribe<TEvent>(eventHandlerFunc);
        }

        public void Unsubscribe<TEvent>(params Func<TEvent, bool>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            foreach (var eventHandlerFunc in eventHandlerFuncs)
                Unsubscribe<TEvent>(eventHandlerFunc);
        }

        public void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent
        {
            lock (sync)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null)
                        handlers.Clear();
                }
            }
        }

        public void UnsubscribeAll()
        {
            lock (sync)
            {
                eventHandlers.Clear();
            }
        }

        public IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            if (eventHandlers.ContainsKey(eventType))
            {
                var handlers = eventHandlers[eventType];
                if (handlers != null)
                    return handlers.Select(p => p as IEventHandler<TEvent>).ToList();
                else
                    return null;
            }
            else
                return null;
        }

        public void Publish<TEvent>(TEvent evnt)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            if (eventHandlers.ContainsKey(eventType) &&
                eventHandlers[eventType] != null &&
                eventHandlers[eventType].Count > 0)
            {
                var handlers = eventHandlers[eventType];
                foreach (var handler in handlers)
                {
                    var eventHandler = handler as IEventHandler<TEvent>;
                    if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                    {
                        Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt);
                    }
                    else
                    {
                        eventHandler.Handle(evnt);
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent evnt, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent
        {
            if (evnt == null)
                throw new ArgumentNullException("evnt");
            var eventType = evnt.GetType();
            if (eventHandlers.ContainsKey(eventType) &&
                eventHandlers[eventType] != null &&
                eventHandlers[eventType].Count > 0)
            {
                var handlers = eventHandlers[eventType];
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew((o) => eventHandler.Handle((TEvent)o), evnt));
                        }
                        else
                        {
                            eventHandler.Handle(evnt);
                        }
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(evnt, true, null);
                }
                catch (Exception ex)
                {
                    callback(evnt, false, ex);
                }
            }
            else
                callback(evnt, false, null);
        }
    }
}
