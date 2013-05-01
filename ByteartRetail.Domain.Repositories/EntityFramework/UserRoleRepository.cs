using ByteartRetail.Domain.Model;
using System;
using System.Linq;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    public class UserRoleRepository : EntityFrameworkRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IRepositoryContext context)
            : base(context) { }

        public Role GetRoleForUser(User user)
        {
            var context = EFContext.Context as ByteartRetailDbContext;
            if (context != null)
            {
                var query = from role in context.Roles
                            from userRole in context.UserRoles
                            from usr in context.Users
                            where role.ID == userRole.RoleID &&
                                usr.ID == userRole.UserID &&
                                usr.ID == user.ID
                            select role;
                return query.FirstOrDefault();
            }
            throw new InvalidOperationException("The provided repository context is invalid.");
        }
    }
}
