using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示订单确认的领域事件。
    /// </summary>
    [Serializable]
    public class OrderConfirmedEvent : DomainEvent
    {
        #region Ctor
        public OrderConfirmedEvent() { }
        public OrderConfirmedEvent(IEntity source) : base(source) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置订单确认的日期。
        /// </summary>
        public DateTime ConfirmedDate { get; set; }
        public string UserEmailAddress { get; set; }
        public Guid OrderID { get; set; }
        #endregion
    }
}
