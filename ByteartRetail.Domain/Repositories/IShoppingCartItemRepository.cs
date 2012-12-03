using ByteartRetail.Domain.Model;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示用于“购物篮项目”聚合根的仓储接口。
    /// </summary>
    public interface IShoppingCartItemRepository : IRepository<ShoppingCartItem>
    {
        #region Methods
        /// <summary>
        /// 根据指定的购物篮以及存在于购物篮中的商品，查找购物篮项目。
        /// </summary>
        /// <param name="shoppingCart">购物篮。</param>
        /// <param name="product">商品。</param>
        /// <returns>购物篮项目。</returns>
        ShoppingCartItem FindItem(ShoppingCart shoppingCart, Product product);
        /// <summary>
        /// 查找指定购物篮中的所有购物篮项目。
        /// </summary>
        /// <param name="cart">购物篮。</param>
        /// <returns>所有购物篮项目。</returns>
        IEnumerable<ShoppingCartItem> FindItemsByCart(ShoppingCart cart);
        #endregion
    }
}
