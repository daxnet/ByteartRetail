using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示领域事件聚合器。
    /// </summary>
    public static class DomainEventAggregator
    {
        #region Private Static Fields
        private static readonly Dictionary<Type, List<Delegate>> domainEventHandlers = new Dictionary<Type, List<Delegate>>();
        #endregion

        #region Public Static Methods
        /// <summary>
        /// 发布一个领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要发布的领域事件的类型。</typeparam>
        /// <param name="evnt">需要发布的领域事件。</param>
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
        /// <summary>
        /// 订阅一个领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要订阅的领域事件的类型。</typeparam>
        /// <param name="domainEventHandler">订阅了指定类型领域事件的事件处理器。</param>
        public static void Subscribe<TEvent>(IDomainEventHandler<TEvent> domainEventHandler)
            where TEvent : class, IDomainEvent
        {
            Subscribe<TEvent>(p => domainEventHandler.Handle(p));
        }
        /// <summary>
        /// 订阅一个领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要订阅的领域事件的类型。</typeparam>
        /// <param name="domainEventHandlerAction">订阅了指定类型领域事件的事件处理函数委托。</param>
        public static void Subscribe<TEvent>(Action<TEvent> domainEventHandlerAction)
            where TEvent : class, IDomainEvent
        {
            if (domainEventHandlers.ContainsKey(typeof(TEvent)))
            {
                List<Delegate> delegates = domainEventHandlers[typeof(TEvent)];
                if (!delegates.Contains(domainEventHandlerAction))
                    delegates.Add(domainEventHandlerAction);
            }
            else
                domainEventHandlers.Add(typeof(TEvent), new List<Delegate> { domainEventHandlerAction });
        }
        #endregion
    }
}
