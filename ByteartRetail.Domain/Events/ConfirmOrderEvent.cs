using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示订单确认的领域事件。
    /// </summary>
    public class ConfirmOrderEvent : DomainEvent
    {
        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>OrderConfirmedEvent</c>类型的实例。
        /// </summary>
        /// <param name="source">产生领域事件的事件源对象。</param>
        public ConfirmOrderEvent(IEntity source) : base(source) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置订单确认的日期。
        /// </summary>
        public DateTime ConfirmedDate { get; set; }
        #endregion
    }
}
