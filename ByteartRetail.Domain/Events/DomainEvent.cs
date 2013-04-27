using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承于该类的类型为领域事件。
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity source;
        private readonly Guid id = Guid.NewGuid();
        private readonly DateTime timeStamp = DateTime.UtcNow;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>DomainEvent</c>类型的实例。
        /// </summary>
        /// <param name="source">产生领域事件的事件源对象。</param>
        public DomainEvent(IEntity source)
        {
            this.source = source;
        }
        #endregion

        #region IDomainEvent Members
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return id; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        /// <summary>
        /// 获取产生领域事件的事件源对象。
        /// </summary>
        public IEntity Source
        {
            get { return source; }
        }
        #endregion

        #region Public Static Methods

        public static void Subscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandler);
        }

        public static void Subscribe<TDomainEvent>(IEnumerable<IDomainEventHandler<TDomainEvent>> domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandlers);
        }

        public static void Subscribe<TDomainEvent>(params IDomainEventHandler<TDomainEvent>[] domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandlers);
        }

        public static void Subscribe<TDomainEvent>(Func<TDomainEvent, bool> domainEventHandlerFunc)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public static void Subscribe<TDomainEvent>(IEnumerable<Func<TDomainEvent, bool>> domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandlerFuncs);
        }

        public static void Subscribe<TDomainEvent>(params Func<TDomainEvent, bool>[] domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Subscribe<TDomainEvent>(domainEventHandlerFuncs);
        }

        public static void Unsubscribe<TDomainEvent>(IDomainEventHandler<TDomainEvent> domainEventHandler)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandler);
        }

        public static void Unsubscribe<TDomainEvent>(IEnumerable<IDomainEventHandler<TDomainEvent>> domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandlers);
        }

        public static void Unsubscribe<TDomainEvent>(params IDomainEventHandler<TDomainEvent>[] domainEventHandlers)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandlers);
        }

        public static void Unsubscribe<TDomainEvent>(Func<TDomainEvent, bool> domainEventHandlerFunc)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandlerFunc);
        }

        public static void Unsubscribe<TDomainEvent>(IEnumerable<Func<TDomainEvent, bool>> domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandlerFuncs);
        }

        public static void Unsubscribe<TDomainEvent>(params Func<TDomainEvent, bool>[] domainEventHandlerFuncs)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Unsubscribe<TDomainEvent>(domainEventHandlerFuncs);
        }

        public static IEnumerable<IDomainEventHandler<TDomainEvent>> GetSubscriptions<TDomainEvent>()
            where TDomainEvent : class, IDomainEvent
        {
            return DomainEventAggregator.Instance.GetSubscriptions<TDomainEvent>();
        }

        public static void UnsubscribeAll<TDomainEvent>()
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.UnsubscribeAll<TDomainEvent>();
        }

        public static void UnsubscribeAll()
        {
            DomainEventAggregator.Instance.UnsubscribeAll();
        }

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Publish<TDomainEvent>(domainEvent);
        }

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            DomainEventAggregator.Instance.Publish<TDomainEvent>(domainEvent, callback, timeout);
        }
        #endregion
    }
}
