using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Specifications;

namespace ByteartRetail.Domain.Repositories.Specifications
{
    internal abstract class UserStringEqualsSpecification : Specification<User>
    {
        protected readonly string value;

        public UserStringEqualsSpecification(string value)
        {
            this.value = value;
        }
    }
}
