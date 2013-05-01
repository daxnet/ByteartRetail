using ByteartRetail.Domain.Model;
using System;
using System.Linq;
using MongoDB.Driver.Linq;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class UserRoleRepository : MongoDBRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IRepositoryContext context)
            : base(context) { }

        public Role GetRoleForUser(User user)
        {
            var userRoleCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(UserRole));
            var userRole = userRoleCollection.AsQueryable<UserRole>().Where(ur => ur.UserID == user.ID).FirstOrDefault();
            if (userRole == null)
                return null;
            var roleCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Role));
            return roleCollection.AsQueryable<Role>().Where(r => r.ID == userRole.RoleID).FirstOrDefault();
        }
    }
}
