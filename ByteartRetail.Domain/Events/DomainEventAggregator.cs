using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示领域事件聚合器。
    /// </summary>
    public static class DomainEventAggregator
    {
        #region Private Static Fields
        private static readonly Dictionary<Type, List<object>> domainEventHandlers = new Dictionary<Type, List<object>>();
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
                var handlers = domainEventHandlers[typeof(TEvent)];
                foreach (var handler in handlers)
                {
                    (handler as IDomainEventHandler<TEvent>).Handle(evnt);
                }
            }
        }
        /// <summary>
        /// 发布一个领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要发布的领域事件的类型。</typeparam>
        /// <param name="evnt">需要发布的领域事件。</param>
        public static async Task PublishAsync<TEvent>(TEvent evnt)
            where TEvent : class, IDomainEvent
        {
            if (domainEventHandlers.ContainsKey(typeof(TEvent)))
            {
                var handlers = domainEventHandlers[typeof(TEvent)];
                await Task.WhenAll(handlers.Select(p => (p as IDomainEventHandler<TEvent>).HandleAsync(evnt)));
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
            if (domainEventHandlers.ContainsKey(typeof(TEvent)))
            {
                var handlers = domainEventHandlers[typeof(TEvent)];
                if (!handlers.Exists(deh => (deh as IDomainEventHandler<TEvent>).Equals(domainEventHandler)))
                    handlers.Add(domainEventHandler);
            }
            else
                domainEventHandlers.Add(typeof(TEvent), new List<object> { domainEventHandler });
        }
        /// <summary>
        /// 解除领域事件处理器对指定领域事件的订阅。
        /// </summary>
        /// <typeparam name="TEvent">需要解除订阅的领域事件的类型。</typeparam>
        /// <param name="domainEventHandler">需要解除订阅的领域事件处理器。</param>
        public static void Unsubscribe<TEvent>(IDomainEventHandler<TEvent> domainEventHandler)
            where TEvent : class, IDomainEvent
        {
            if (domainEventHandlers.ContainsKey(typeof(TEvent)))
            {
                var handlers = domainEventHandlers[typeof(TEvent)];
                if (handlers.Exists(deh => (deh as IDomainEventHandler<TEvent>).Equals(domainEventHandler)))
                {
                    var handlerToRemove = handlers.First(deh => (deh as IDomainEventHandler<TEvent>).Equals(domainEventHandler));
                    handlers.Remove(handlerToRemove);
                }
            }
        }
        /// <summary>
        /// 订阅一个领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要订阅的领域事件的类型。</typeparam>
        /// <param name="domainEventHandlerAction">订阅了指定类型领域事件的事件处理函数委托。</param>
        public static void Subscribe<TEvent>(Action<TEvent> domainEventHandlerAction)
            where TEvent : class, IDomainEvent
        {
            Subscribe<TEvent>(new ActionDelegatedDomainEventHandler<TEvent>(domainEventHandlerAction));
        }
        /// <summary>
        /// 解除领域事件处理委托对指定领域事件的订阅。
        /// </summary>
        /// <typeparam name="TEvent">需要解除订阅的领域事件的类型。</typeparam>
        /// <param name="domainEventHandler">需要解除订阅的领域事件处理委托。</param>
        public static void Unsubscribe<TEvent>(Action<TEvent> domainEventHandlerAction)
            where TEvent : class, IDomainEvent
        {
            Unsubscribe<TEvent>(new ActionDelegatedDomainEventHandler<TEvent>(domainEventHandlerAction));
        }
        #endregion
    }
}
