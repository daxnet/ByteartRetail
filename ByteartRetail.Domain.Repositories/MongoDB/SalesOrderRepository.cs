using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.Specifications;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class SalesOrderRepository : MongoDBRepository<SalesOrder>, ISalesOrderRepository
    {
        public SalesOrderRepository(IRepositoryContext context)
            : base(context)
        { }

        #region ISalesOrderRepository Members

        public IEnumerable<SalesOrder> FindSalesOrdersByUser(User user)
        {
            return FindAll(new SalesOrderBelongsToUserSpecification(user), sp => sp.DateCreated, SortOrder.Descending);
        }

        public SalesOrder GetSalesOrderByID(Guid orderID)
        {
            return Get(new SalesOrderIDEqualsSpecification(orderID), elp => elp.SalesLines);
        }

        #endregion
    }
}
