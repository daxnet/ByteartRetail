using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal class ShoppingCartItemHasProductSpecification : Specification<ShoppingCartItem>
    {

        private readonly Product product;

        public ShoppingCartItemHasProductSpecification(Product product)
        {
            this.product = product;
        }

        public override System.Linq.Expressions.Expression<Func<ShoppingCartItem, bool>> GetExpression()
        {
            return p => p.Product.ID == product.ID;
        }
    }
}
