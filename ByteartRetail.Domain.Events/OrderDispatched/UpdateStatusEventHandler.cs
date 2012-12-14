using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Events.OrderDispatched
{
    /// <summary>
    /// 表示更新订单状态的事件处理器。
    /// </summary>
    public class UpdateStatusEventHandler : IEventHandler<OrderDispatchedEvent>
    {
        #region IEventHandler<OrderDispatchedEvent> Members
        /// <summary>
        /// 处理给定的领域事件。
        /// </summary>
        /// <param name="event">需要处理的领域事件。</param>
        public void Handle(OrderDispatchedEvent @event)
        {
            @event.DispatchedOrder.DateDispatched = @event.DispatchedDate;
            @event.DispatchedOrder.Status = SalesOrderStatus.Dispatched;
        }

        #endregion
    }
}
