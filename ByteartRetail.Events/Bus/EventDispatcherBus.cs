using ByteartRetail.Events.Handlers;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示一种基于事件派发机制的事件总线。
    /// </summary>
    public class EventDispatcherBus : DisposableObject, IBus
    {
        #region Private Fields
        private readonly List<IEventDispatcher> eventDispatchers = new List<IEventDispatcher>();
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>EventDispatcherBus</c>类型的实例。
        /// </summary>
        /// <param name="eventDispatchers">需要向当前事件总线注册的事件派发器。</param>
        public EventDispatcherBus(IEventDispatcher[] eventDispatchers)
        {
            this.eventDispatchers.AddRange(eventDispatchers);
        }
        #endregion

        #region Private Methods
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
        #endregion

        #region Protected Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UnsubscribeAll();
            }
        }
        #endregion

        #region IBus Members
        /// <summary>
        /// 向事件总线发布事件。
        /// </summary>
        /// <typeparam name="TEvent">需要发布的事件类型。</typeparam>
        /// <param name="evnt">需要发布的事件。</param>
        public void Publish<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            var eventDispatcher = GetEventDispatcher(evnt.GetType());
            if (eventDispatcher != null && eventDispatcher.IsEnabled)
                eventDispatcher.DispatchEvent(evnt);
        }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前的事件总线是否支持分布式事务处理。
        /// </summary>
        public bool IsDistributedTransactionSupported
        {
            get { return false; }
        }
        /// <summary>
        /// 向事件总线订阅指定事件类型的事件处理器。
        /// </summary>
        /// <typeparam name="TEvent">需要订阅的事件类型。</typeparam>
        /// <param name="eventHandler">订阅指定类型的事件的事件处理器。</param>
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
        /// <summary>
        /// 从事件总线中退订指定事件类型的事件处理过程。
        /// </summary>
        /// <typeparam name="TEvent">需要退订的事件类型。</typeparam>
        /// <param name="eventHandler">退订指定类型的事件的事件处理器。</param>
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : class, IEvent
        {
            IEventDispatcher<TEvent> eventDispatcher = GetEventDispatcher<TEvent>();
            if (eventDispatcher != null)
            {
                eventDispatcher.UnregisterHandler(eventHandler);
            }
        }
        /// <summary>
        /// 退订所有事件的事件处理器。
        /// </summary>
        public void UnsubscribeAll()
        {
            foreach (var eventDispatcher in this.eventDispatchers)
                eventDispatcher.ClearHandlers();
            this.eventDispatchers.Clear();
        }
        #endregion

    }
}
