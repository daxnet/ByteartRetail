using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“销售订单明细”的领域实体对象。
    /// </summary>
    public class SalesLine : IEntity
    {
        #region Private Fields
        private Guid id;
        private Product product;
        private int quantity;
        private SalesOrder salesOrder;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个<c>SalesLine</c>类型的实例。
        /// </summary>
        public SalesLine() { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置当前订单明细中所包含的商品数量。
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        /// <summary>
        /// 获取或设置属于当前订单明细的商品对象。
        /// </summary>
        public virtual Product Product
        {
            get { return product; }
            set { product = value; }
        }
        /// <summary>
        /// 获取或设置包含了当前订单明细的销售订单对象。
        /// </summary>
        
        public virtual SalesOrder SalesOrder
        {
            get { return salesOrder; }
            set { salesOrder = value; }
        }

        /// <summary>
        /// 获取当前订单明细的小计金额。
        /// </summary>
        /// <remarks>在严格的业务系统中，金额往往以Money模式实现。有关Money模式，请参见：http://martinfowler.com/eaaCatalog/money.html
        /// </remarks>
        public decimal LineAmount
        {
            get { return this.Product.UnitPrice * this.quantity; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 确定指定的Object是否等于当前的Object。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的Object与当前Object相等，则返回true，否则返回false。</returns>
        /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.equals。
        /// </remarks>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            SalesLine other = obj as SalesLine;
            if ((object)other == null)
                return false;
            return this.ID == other.ID;
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前Object的哈希代码。</returns>
        /// <remarks>有关此函数的更多信息，请参见：http://msdn.microsoft.com/zh-cn/library/system.object.gethashcode。
        /// </remarks>
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }
        #endregion

        #region Public Static Operator Overrides
        /// <summary>
        /// 确定两个“销售订单明细”对象是否相等。
        /// </summary>
        /// <param name="a">待确定的第一个“销售订单明细”对象。</param>
        /// <param name="b">待确定的另一个“销售订单明细”对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public static bool operator ==(SalesLine a, SalesLine b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }
            return a.Equals(b);
        }

        /// <summary>
        /// 确定两个“销售订单明细”对象是否不相等。
        /// </summary>
        /// <param name="a">待确定的第一个“销售订单明细”对象。</param>
        /// <param name="b">待确定的另一个“销售订单明细”对象。</param>
        /// <returns>如果两者不相等，则返回true，否则返回false。</returns>
        public static bool operator !=(SalesLine a, SalesLine b)
        {
            return !(a == b);
        }
        #endregion

        #region IEntity Members
        /// <summary>
        /// 获取或设置当前实体对象的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }

        #endregion
    }
}
