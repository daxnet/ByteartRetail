using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Services
{
    /// <summary>
    /// 表示继承于该接口的类型都是领域服务（Domain Service）类型。
    /// </summary>
    public interface IDomainService
    {
        #region Methods
        /// <summary>
        /// 将指定的商品归类到指定的商品分类中。
        /// </summary>
        /// <param name="product">需要归类的商品。</param>
        /// <param name="category">商品分类。</param>
        /// <returns>用以表述商品及其分类之间关系的实体。</returns>
        Categorization Categorize(Product product, Category category);
        /// <summary>
        /// 将指定的商品从其所属的商品分类中移除。
        /// </summary>
        /// <param name="product">商品。</param>
        /// <param name="category">分类，若为NULL，则表示从所有分类中移除。</param>
        void Uncategorize(Product product, Category category = null);
        /// <summary>
        /// 通过指定的用户及其所拥有的购物篮实体，创建销售订单。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="shoppingCart">购物篮实体。</param>
        /// <returns>销售订单实体。</returns>
        SalesOrder CreateSalesOrder(User user, ShoppingCart shoppingCart);
        /// <summary>
        /// 将指定的用户赋予特定的角色。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="role">角色实体。</param>
        /// <returns>用以表述用户及其角色之间关系的实体。</returns>
        UserRole AssignRole(User user, Role role);
        /// <summary>
        /// 将指定的用户从角色中移除。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="role">角色实体，若为NULL，则表示从所有角色中移除。</param>
        void UnassignRole(User user, Role role = null);
        #endregion
    }
}
