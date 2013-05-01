using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    public class CategoryRepository : EntityFrameworkRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IRepositoryContext context) : base(context) { }
    }
}
