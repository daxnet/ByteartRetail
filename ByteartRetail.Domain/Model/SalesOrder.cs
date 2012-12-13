using ByteartRetail.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“销售订单”的领域实体对象。
    /// </summary>
    public class SalesOrder : AggregateRoot
    {
        #region Private Fields
        private SalesOrderStatus status;
        private DateTime dateCreated;
        private DateTime? dateDispatched;
        private DateTime? dateDelivered;
        private User user;
        private List<SalesLine> salesLines = new List<SalesLine>();
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个<c>SalesOrder</c>类型实例。
        /// </summary>
        public SalesOrder()
        {
            dateCreated = DateTime.Now;
            status = SalesOrderStatus.Created;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置销售订单的状态。
        /// </summary>
        public SalesOrderStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary>
        /// 获取或设置销售订单的创建日期。
        /// </summary>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        /// <summary>
        /// 获取或设置销售订单的发货日期。
        /// </summary>
        public DateTime? DateDispatched
        {
            get { return dateDispatched; }
            set { dateDispatched = value; }
        }

        /// <summary>
        /// 获取或设置销售订单的派送日期。
        /// </summary>
        public DateTime? DateDelivered
        {
            get { return dateDelivered; }
            set { dateDelivered = value; }
        }

        /// <summary>
        /// 获取或设置销售订单的订单明细。
        /// </summary>
        public virtual List<SalesLine> SalesLines
        {
            get { return salesLines; }
            set { salesLines = value; }
        }

        /// <summary>
        /// 获取或设置拥有该销售订单的客户实体。
        /// </summary>
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// 获取该销售订单的派送地址。
        /// </summary>
        public Address DeliveryAddress
        {
            get { return user.DeliveryAddress; }
        }

        /// <summary>
        /// 获取该销售订单的金额。
        /// </summary>
        /// <remarks>在严格的业务系统中，金额往往以Money模式实现。有关Money模式，请参见：http://martinfowler.com/eaaCatalog/money.html
        /// </remarks>
        public decimal Subtotal
        {
            get
            {
                return this.salesLines.Sum(p => p.LineAmount);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 当客户完成收货后，对销售订单进行确认。
        /// </summary>
        public void Confirm()
        {
            this.status = SalesOrderStatus.Delivered;
            this.dateDelivered = DateTime.Now;
        }

        /// <summary>
        /// 处理发货。
        /// </summary>
        public void Dispatch()
        {
            //this.status = SalesOrderStatus.Dispatched;
            //this.dateDispatched = DateTime.Now;
            EventDispatcher.DispatchEvent<OrderDispatchedEvent>(new OrderDispatchedEvent
            {
                DispatchedDate = DateTime.Now,
                DispatchedOrder = this
            });
        }
        #endregion

    }
}
