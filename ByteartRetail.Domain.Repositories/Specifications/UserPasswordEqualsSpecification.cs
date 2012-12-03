using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal class UserPasswordEqualsSpecification : UserStringEqualsSpecification
    {

        public UserPasswordEqualsSpecification(string password)
            : base(password)
        {
        }

        public override System.Linq.Expressions.Expression<Func<User, bool>> GetExpression()
        {
            return c => c.Password == value;
        }
    }
}
