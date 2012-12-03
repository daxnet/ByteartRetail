using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.Specifications;
using ByteartRetail.Domain.Repositories.EntityFramework;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories
{
    public class ShoppingCartItemRepository : EntityFrameworkRepository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(IRepositoryContext context)
            : base(context)
        { }

        #region IShoppingCartItemRepository Members

        public ShoppingCartItem FindItem(ShoppingCart shoppingCart, Product product)
        {
            return Find(Specification<ShoppingCartItem>.Eval(sci => sci.ShoppingCart.ID == shoppingCart.ID &&
                sci.Product.ID == product.ID), elp => elp.Product);
        }

        public IEnumerable<ShoppingCartItem> FindItemsByCart(ShoppingCart cart)
        {
            return FindAll(new ShoppingCartItemBelongsToShoppingCartSpecification(cart), elp => elp.Product);
        }

        #endregion
    }
}
