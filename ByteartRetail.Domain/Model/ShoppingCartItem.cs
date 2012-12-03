using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“购物篮项目”的领域实体对象。
    /// </summary>
    public class ShoppingCartItem : AggregateRoot
    {
        #region Private Fields
        private int quantity;
        private Product product;
        private ShoppingCart shoppingCart;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个<c>ShoppingCartItem</c>类型的实例。
        /// </summary>
        public ShoppingCartItem() { }
        
        /// <summary>
        /// 初始化一个<c>ShoppingCartItem</c>类型的实例。
        /// </summary>
        /// <param name="product">属于该购物篮项目的笔记本电脑商品实体。</param>
        /// <param name="shoppingCart">拥有该购物篮项目的购物篮实体。</param>
        public ShoppingCartItem(Product product, ShoppingCart shoppingCart, int quantity)
        {
            this.quantity = quantity;
            this.product = product;
            this.shoppingCart = shoppingCart;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置属于当前购物篮项目的笔记本电脑商品实体。
        /// </summary>
        public virtual Product Product
        {
            get { return product; }
            set { product = value; }
        }

        /// <summary>
        /// 获取或设置拥有当前购物篮项目的购物篮实体。
        /// </summary>
        public virtual ShoppingCart ShoppingCart
        {
            get { return shoppingCart; }
            set { shoppingCart = value; }
        }

        /// <summary>
        /// 获取或设置当前购物篮项目所包含的笔记本电脑商品的个数。
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 获取或设置当前购物篮项目的金额。
        /// </summary>
        /// <remarks>在严格的业务系统中，金额往往以Money模式实现。有关Money模式，请参见：http://martinfowler.com/eaaCatalog/money.html
        /// </remarks>
        public decimal LineAmount
        {
            get
            {
                return this.product.UnitPrice * this.quantity;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 将当前的购物篮项目转换为销售订单行。
        /// </summary>
        /// <returns></returns>
        public SalesLine ConvertToSalesLine()
        {
            SalesLine salesLine = new SalesLine();
            salesLine.ID = Guid.NewGuid(); // 为每个SalesLine设置一个不同的ID，以便EF的Context能够识别不同的SalesLine
            salesLine.Product = this.Product;
            salesLine.Quantity = this.Quantity;
            return salesLine;
        }

        /// <summary>
        /// 更新当前购物篮项目所包含的笔记本电脑商品的个数。
        /// </summary>
        /// <param name="quantity">需要更新的笔记本电脑商品的个数。</param>
        public void UpdateQuantity(int quantity)
        {
            this.quantity = quantity;
        }
        #endregion
    }
}
