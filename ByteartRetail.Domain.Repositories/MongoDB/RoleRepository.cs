using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class RoleRepository : MongoDBRepository<Role>, IRoleRepository
    {
        public RoleRepository(IRepositoryContext context)
            : base(context)
        { }

    }
}
