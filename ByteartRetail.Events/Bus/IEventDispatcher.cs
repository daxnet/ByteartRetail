
using ByteartRetail.Events.Handlers;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示实现该接口的类型为事件派发器。
    /// </summary>
    public interface IEventDispatcher
    {
        #region Properties
        /// <summary>
        /// 获取事件的派发方式。
        /// </summary>
        EventDispatchMode DispatchMode { get; }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前的事件派发器是否已启用。
        /// </summary>
        bool IsEnabled { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 向事件派发器注册用于处理<c>IEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        void RegisterHandler(IEventHandler<IEvent> eventHandler);
        /// <summary>
        /// 将指定的事件处理器从事件派发器中注销。
        /// </summary>
        /// <param name="eventHandler">需要注销的事件处理器。</param>
        void UnregisterHandler(IEventHandler<IEvent> eventHandler);
        /// <summary>
        /// 注销所有的事件处理器。
        /// </summary>
        void ClearHandlers();
        /// <summary>
        /// 派发事件。
        /// </summary>
        /// <param name="evnt">需要派发的事件。</param>
        void DispatchEvent(IEvent evnt);
        #endregion
    }

    /// <summary>
    /// 表示实现该接口的类型为事件派发器。
    /// </summary>
    /// <typeparam name="TEvent">事件派发器所能派发的事件类型。</typeparam>
    public interface IEventDispatcher<TEvent> : IEventDispatcher
        where TEvent: class, IEvent
    {
        #region Methods
        /// <summary>
        /// 向事件派发器注册用于处理<c>TEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        void RegisterHandler(IEventHandler<TEvent> eventHandler);
        /// <summary>
        /// 将指定的事件处理器从事件派发器中注销。
        /// </summary>
        /// <param name="eventHandler">需要注销的事件处理器。</param>
        void UnregisterHandler(IEventHandler<TEvent> eventHandler);
        /// <summary>
        /// 派发事件。
        /// </summary>
        /// <param name="evnt">需要派发的事件。</param>
        void DispatchEvent(TEvent evnt);
        #endregion
    }
}
