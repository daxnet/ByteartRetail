
namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示实现该接口的类型为Event Aggregator类型。
    /// </summary>
    /// <remarks>有关Event Aggreator模式的更多信息，请参考：http://martinfowler.com/eaaDev/EventAggregator.html。
    /// </remarks>
    public interface IEventAggregator
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
        void RegisterHandler(IEventHandler<IDomainEvent> eventHandler);
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="domainEvent">需要派发的领域事件。</param>
        void DispatchEvent(IDomainEvent domainEvent);
        #endregion
    }

    /// <summary>
    /// 表示实现该接口的类型为Event Aggregator类型。
    /// </summary>
    /// <typeparam name="TEvent">领域事件的类型。</typeparam>
    /// <remarks>此接口为<see cref="IEventAggregator"/>接口的泛型版本。
    /// 有关Event Aggreator模式的更多信息，请参考：http://martinfowler.com/eaaDev/EventAggregator.html。
    /// </remarks>
    public interface IEventAggregator<TEvent> : IEventAggregator
        where TEvent: class, IDomainEvent
    {
        #region Methods
        /// <summary>
        /// 向Event Aggreator注册用于处理<c>TEvent</c>类型的事件处理器。
        /// </summary>
        /// <param name="eventHandler">需要注册的事件处理器。</param>
        void RegisterHandler(IEventHandler<TEvent> eventHandler);
        /// <summary>
        /// 派发领域事件。
        /// </summary>
        /// <param name="domainEvent">需要派发的领域事件。</param>
        void DispatchEvent(TEvent domainEvent);
        #endregion
    }
}
