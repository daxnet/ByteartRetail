using ByteartRetail.Events.Handlers;
using System;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示实现该接口的类型为事件总线类型。
    /// </summary>
    public interface IBus : IDisposable
    {
        #region Methods
        /// <summary>
        /// 向事件总线发布事件。
        /// </summary>
        /// <typeparam name="TEvent">需要发布的事件类型。</typeparam>
        /// <param name="evnt">需要发布的事件。</param>
        void Publish<TEvent>(TEvent evnt)
            where TEvent : class, IEvent;
        /// <summary>
        /// 向事件总线订阅指定事件类型的事件处理器。
        /// </summary>
        /// <typeparam name="TEvent">需要订阅的事件类型。</typeparam>
        /// <param name="eventHandler">订阅指定类型的事件的事件处理器。</param>
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        /// <summary>
        /// 从事件总线中退订指定事件类型的事件处理过程。
        /// </summary>
        /// <typeparam name="TEvent">需要退订的事件类型。</typeparam>
        /// <param name="eventHandler">退订指定类型的事件的事件处理器。</param>
        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        /// <summary>
        /// 退订所有事件的事件处理器。
        /// </summary>
        void UnsubscribeAll();
        #endregion

        #region Properties
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前的事件总线是否支持分布式事务处理。
        /// </summary>
        bool IsDistributedTransactionSupported { get; }
        #endregion
    }
}
