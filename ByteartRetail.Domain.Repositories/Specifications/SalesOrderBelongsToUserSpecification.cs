using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal class SalesOrderBelongsToUserSpecification : Specification<SalesOrder>
    {
        private readonly User user;

        public SalesOrderBelongsToUserSpecification(User user)
        {
            this.user = user;
        }

        public override System.Linq.Expressions.Expression<Func<SalesOrder, bool>> GetExpression()
        {
            return so => so.User.ID == user.ID;
        }
    }
}
