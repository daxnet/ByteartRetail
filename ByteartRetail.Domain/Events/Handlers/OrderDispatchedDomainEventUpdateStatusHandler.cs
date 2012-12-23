using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Events.Handlers
{
    /// <summary>
    /// 表示用于处理订单已发货的领域事件的事件处理器。
    /// </summary>
    public class OrderDispatchedDomainEventUpdateStatusHandler : DomainEventHandler<OrderDispatchedEvent>
    {
        #region Public Methods
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        public override void Handle(OrderDispatchedEvent evnt)
        {
            SalesOrder salesOrder = evnt.Source as SalesOrder;
            salesOrder.DateDispatched = evnt.DispatchedDate;
            salesOrder.Status = SalesOrderStatus.Dispatched;
        }

        #endregion
    }
}
