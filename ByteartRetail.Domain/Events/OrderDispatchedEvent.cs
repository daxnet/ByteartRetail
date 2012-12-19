using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示当针对某销售订单进行发货时所产生的领域事件。
    /// </summary>
    public class OrderDispatchedEvent : DomainEvent
    {
        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>OrderDispatchedEvent</c>类型的实例。
        /// </summary>
        /// <param name="source">产生领域事件的事件源对象。</param>
        public OrderDispatchedEvent(IEntity source) : base(source) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置订单发货的日期。
        /// </summary>
        public DateTime DispatchedDate { get; set; }
        #endregion
    }
}
