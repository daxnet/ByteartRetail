using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal class UserNameEqualsSpecification : UserStringEqualsSpecification
    {
        public UserNameEqualsSpecification(string userName)
            : base(userName)
        {

        }

        public override System.Linq.Expressions.Expression<Func<User, bool>> GetExpression()
        {
            return c => c.UserName == value;
        }
    }
}
