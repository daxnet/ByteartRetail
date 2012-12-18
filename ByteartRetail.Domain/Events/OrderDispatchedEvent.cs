using ByteartRetail.Domain.Model;
using ByteartRetail.Events;
using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示当针对某销售订单进行发货时所产生的领域事件。
    /// </summary>
    public class OrderDispatchedEvent : DomainEvent
    {
        public OrderDispatchedEvent(IEntity source) : base(source) { }

        #region Properties
        /// <summary>
        /// 获取或设置订单发货的日期。
        /// </summary>
        public DateTime DispatchedDate { get; set; }
        #endregion
    }
}
