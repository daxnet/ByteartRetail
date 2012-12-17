
namespace ByteartRetail.Events.Handlers
{
    /// <summary>
    /// 表示实现该接口的类型为领域事件处理器。
    /// </summary>
    /// <typeparam name="TEvent">领域事件的类型。</typeparam>
    public interface IEventHandler<in TEvent>
        where TEvent : class, IEvent
    {
        #region Methods
        /// <summary>
        /// 处理给定的领域事件。
        /// </summary>
        /// <param name="event">需要处理的领域事件。</param>
        void Handle(TEvent @event);
        #endregion
    }
}
