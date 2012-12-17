using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示事件派发器。
    /// </summary>
    public static class EventDispatcher
    {
        #region Private Fields
        private static Dictionary<Type, IEventAggregator> eventAggregators = new Dictionary<Type, IEventAggregator>();
        #endregion

        #region Public Methods
        /// <summary>
        /// 向事件派发器注册给定事件类型的事件聚合器。
        /// </summary>
        /// <typeparam name="TEvent">需要注册的事件类型。</typeparam>
        public static void RegisterAggregator<TEvent>()
            where TEvent : class, IEvent
        {
            if (eventAggregators.ContainsKey(typeof(TEvent)))
                return;
            IEventAggregator<TEvent> eventAggregator = ServiceLocator
                .Instance
                .GetService<IEventAggregator<TEvent>>(); // 从IoC容器中解析应用于给定事件类型的事件聚合器实例。

            eventAggregators.Add(typeof(TEvent), eventAggregator); // 将事件聚合器实例添加到线程的本地存储中。
        }
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <typeparam name="TEvent">需要派发的领域事件的类型。</typeparam>
        /// <param name="event">需要派发的领域事件。</param>
        public static void DispatchEvent<TEvent>(TEvent @event)
            where TEvent : class, IEvent
        {
            var eventAggregator = eventAggregators[typeof(TEvent)];
            if (eventAggregator != null)
                eventAggregator.DispatchEvent(@event);
        }
        #endregion
    }
}
