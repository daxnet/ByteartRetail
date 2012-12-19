using ByteartRetail.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示事件派发器。
    /// </summary>
    /// <typeparam name="TEvent">事件的类型，针对该类型事件的处理器将会被当前事件派发器所派发。</typeparam>
    internal class EventDispatcher<TEvent> : IEventDispatcher<TEvent>
        where TEvent : class, IEvent
    {
        #region Private Fields
        /// <summary>
        /// 保存事件处理器的列表。
        /// </summary>
        private readonly List<IEventHandler<TEvent>> eventHandlers = new List<IEventHandler<TEvent>>();
        /// <summary>
        /// 指定事件派发器向所注册的各事件处理器进行事件派发的方式。
        /// </summary>
        private readonly EventDispatchMode dispatchMode;
        private readonly bool enabled;
        #endregion

        #region Ctor
        /// <summary>
        /// 创建一个新的<c>EventDispatcher{TEvent}</c>类型的实例。
        /// </summary>
        /// <param name="dispatchMode">事件派发器向所注册的各事件处理器进行事件派发的方式。</param>
        /// <param name="enabled">表示当前事件派发器是否启用。</param>
        /// <param name="eventHandlers">事件处理器的列表。</param>
        public EventDispatcher(EventDispatchMode dispatchMode, bool enabled, IEventHandler<TEvent>[] eventHandlers)
        {
            this.dispatchMode = dispatchMode;
            this.enabled = enabled;
            foreach (var eh in eventHandlers)
                this.RegisterHandler(eh);
        }
        #endregion

        #region IEventDispatcher<TEvent> Members
        /// <summary>
        /// 向事件派发器注册用于处理<c>TEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        public virtual void RegisterHandler(IEventHandler<TEvent> eventHandler)
        {
            if (!eventHandlers.Contains(eventHandler))
                eventHandlers.Add(eventHandler);
        }
        /// <summary>
        /// 将指定的事件处理器从事件派发器中注销。
        /// </summary>
        /// <param name="eventHandler">需要注销的事件处理器。</param>
        public void UnregisterHandler(IEventHandler<TEvent> eventHandler)
        {
            if (eventHandlers.Contains(eventHandler))
                eventHandlers.Remove(eventHandler);
        }

        /// <summary>
        /// 派发事件。
        /// </summary>
        /// <param name="evnt">需要派发的事件。</param>
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
        /// 向事件派发器注册用于处理<c>IEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        public void RegisterHandler(IEventHandler<IEvent> eventHandler)
        {
            IEventHandler<TEvent> genericEventHandler = eventHandler;
            this.RegisterHandler(genericEventHandler);
        }
        /// <summary>
        /// 将指定的事件处理器从事件派发器中注销。
        /// </summary>
        /// <param name="eventHandler">需要注销的事件处理器。</param>
        public void UnregisterHandler(IEventHandler<IEvent> eventHandler)
        {
            IEventHandler<TEvent> genericEventHandler = eventHandler;
            this.UnregisterHandler(genericEventHandler);
        }
        /// <summary>
        /// 注销所有的事件处理器。
        /// </summary>
        public void ClearHandlers()
        {
            for (int i = 0; i < this.eventHandlers.Count; i++)
                this.eventHandlers[i] = null;
            this.eventHandlers.Clear();
        }

        /// <summary>
        /// 获取事件的派发方式。
        /// </summary>
        public EventDispatchMode DispatchMode
        {
            get { return this.dispatchMode; }
        }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前的事件派发器是否已启用。
        /// </summary>
        public bool IsEnabled
        {
            get { return this.enabled; }
        }
        /// <summary>
        /// 派发事件。
        /// </summary>
        /// <param name="domainEvent">需要派发的事件。</param>
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
