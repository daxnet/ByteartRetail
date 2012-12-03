using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal class ShoppingCartItemBelongsToShoppingCartSpecification : Specification<ShoppingCartItem>
    {
        private readonly ShoppingCart shoppingCart;

        public ShoppingCartItemBelongsToShoppingCartSpecification(ShoppingCart shoppingCart)
        {
            this.shoppingCart = shoppingCart;
        }

        public override System.Linq.Expressions.Expression<Func<ShoppingCartItem, bool>> GetExpression()
        {
            // 注意：在以前的版本中，由于Repository中在获取对象时，Where使用的是Specification.IsSatisifedBy
            // 函数作为参数，就导致EF采用了Where的Func重载版本，于是EF会把所有的数据从数据库中读出，然后在内存
            // 中作筛选。而从V3开始，Repository使用Specification.GetExpression作查询筛选，因此调用的是IQueryable
            // 接口上的Where重载，导致EF到数据库中做数据筛选，但是这种筛选方式不支持对象的比对，所以在这里需要根据
            // ID进行对象相等的认定。
            return p => p.ShoppingCart.ID == shoppingCart.ID;
        }
    }
}
