using ByteartRetail.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示事件聚合器。
    /// </summary>
    /// <typeparam name="TEvent">事件的类型，针对该类型事件的处理器将会被当前事件聚合器所聚合。</typeparam>
    internal class EventDispatcher<TEvent> : IEventDispatcher<TEvent>
        where TEvent : class, IEvent
    {
        #region Protected Fields
        /// <summary>
        /// 保存事件处理器的列表。
        /// </summary>
        protected readonly List<IEventHandler<TEvent>> eventHandlers = new List<IEventHandler<TEvent>>();
        /// <summary>
        /// 指定事件聚合器向所注册的各事件处理器进行事件派发的方式。
        /// </summary>
        protected readonly EventDispatchMode dispatchMode;
        #endregion

        #region Ctor
        /// <summary>
        /// 创建一个新的<c>EventAggregator{TEvent}</c>类型的实例。
        /// </summary>
        /// <param name="dispatchMode">事件聚合器向所注册的各事件处理器进行事件派发的方式。</param>
        /// <param name="eventHandlers">事件处理器的列表。</param>
        public EventDispatcher(EventDispatchMode dispatchMode, IEventHandler<TEvent>[] eventHandlers)
        {
            this.dispatchMode = dispatchMode;
            foreach (var eh in eventHandlers)
                this.RegisterHandler(eh);
        }
        #endregion

        #region IEventDispatcher<TEvent> Members
        /// <summary>
        /// 向Event Aggreator注册用于处理<c>TEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        public virtual void RegisterHandler(IEventHandler<TEvent> eventHandler)
        {
            if (!eventHandlers.Contains(eventHandler))
                eventHandlers.Add(eventHandler);
        }

        public void UnregisterHandler(IEventHandler<TEvent> eventHandler)
        {
            if (eventHandlers.Contains(eventHandler))
                eventHandlers.Remove(eventHandler);
        }

        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="domainEvent">需要派发的领域事件。</param>
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

        #region IEventDispatcher Members
        /// <summary>
        /// 向Event Aggreator注册用于处理<c>IDomainEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        public void RegisterHandler(IEventHandler<IEvent> eventHandler)
        {
            IEventHandler<TEvent> genericEventHandler = eventHandler;
            this.RegisterHandler(genericEventHandler);
        }

        public void UnregisterHandler(IEventHandler<IEvent> eventHandler)
        {
            IEventHandler<TEvent> genericEventHandler = eventHandler;
            this.UnregisterHandler(genericEventHandler);
        }

        /// <summary>
        /// 获取领域事件的派发方式。
        /// </summary>
        public EventDispatchMode DispatchMode
        {
            get { return this.dispatchMode; }
        }
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="domainEvent">需要派发的领域事件。</param>
        public void DispatchEvent(IEvent domainEvent)
        {
            if (domainEvent is TEvent)
            {
                DispatchEvent(domainEvent as TEvent);
            }
        }
        #endregion
    }
}
