
using ByteartRetail.Events.Handlers;
using System;

namespace ByteartRetail.Events.Bus
{
    
    public interface IEventDispatcher
    {
        #region Properties
        /// <summary>
        /// 获取领域事件的派发方式。
        /// </summary>
        EventDispatchMode DispatchMode { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 向Event Aggreator注册用于处理<c>IDomainEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        void RegisterHandler(IEventHandler<IEvent> eventHandler);
        void UnregisterHandler(IEventHandler<IEvent> eventHandler);
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="evnt">需要派发的领域事件。</param>
        void DispatchEvent(IEvent evnt);
        #endregion
    }

    
    public interface IEventDispatcher<TEvent> : IEventDispatcher
        where TEvent: class, IEvent
    {
        #region Methods
        /// <summary>
        /// 向Event Aggreator注册用于处理<c>TEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        void RegisterHandler(IEventHandler<TEvent> eventHandler);
        void UnregisterHandler(IEventHandler<TEvent> eventHandler);
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="evnt">需要派发的领域事件。</param>
        void DispatchEvent(TEvent evnt);
        #endregion
    }
}
