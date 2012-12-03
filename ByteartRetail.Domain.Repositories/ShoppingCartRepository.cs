using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.Specifications;
using ByteartRetail.Domain.Repositories.EntityFramework;

namespace ByteartRetail.Domain.Repositories
{
    public class ShoppingCartRepository : EntityFrameworkRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(IRepositoryContext context)
            : base(context)
        {

        }

        #region IShoppingCartRepository Members

        public ShoppingCart FindShoppingCartByUser(User user)
        {
            return Find(new ShoppingCartBelongsToCustomerSpecification(user));
        }

        #endregion
    }
}
