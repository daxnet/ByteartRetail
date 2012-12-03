using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“购物篮”的领域实体对象。
    /// </summary>
    public class ShoppingCart : AggregateRoot
    {
        #region Private Fields
        private User user;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个<c>ShoppingCart</c>类型的实例。
        /// </summary>
        public ShoppingCart() { }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置拥有此购物篮的客户实体。
        /// </summary>
        public virtual User User
        {
            get { return user; }
            set { user = value; }
        }
        #endregion

    }
}
