using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class CategoryRepository : MongoDBRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IRepositoryContext context) : base(context) { }
    }
}
