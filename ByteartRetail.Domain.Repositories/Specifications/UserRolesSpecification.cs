using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;
using System.Linq.Expressions;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    public class UserRolesSpecification : Specification<UserRole>
    {
        private readonly Guid userID;

        public UserRolesSpecification(User user)
        {
            this.userID = user.ID;
        }

        public override Expression<Func<UserRole, bool>> GetExpression()
        {
            return p => p.UserID == userID;
        }
    }
}
