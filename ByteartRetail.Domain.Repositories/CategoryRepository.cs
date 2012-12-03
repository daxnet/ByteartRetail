using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteartRetail.Domain.Repositories
{
    public class CategoryRepository : EntityFrameworkRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IRepositoryContext context) : base(context) { }
    }
}
