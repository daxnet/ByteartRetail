using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.EntityFramework;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    public class RoleRepository : EntityFrameworkRepository<Role>, IRoleRepository
    {
        public RoleRepository(IRepositoryContext context)
            : base(context)
        { }

    }
}
