using ByteartRetail.Domain.Model;
using ByteartRetail.Events.Bus;
using System.Data.Entity;
using System.Threading;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    public class EntityFrameworkRepositoryContext : RepositoryContext, IEntityFrameworkRepositoryContext
    {
        private readonly ThreadLocal<ByteartRetailDbContext> localCtx = new ThreadLocal<ByteartRetailDbContext>(() => new ByteartRetailDbContext());

        public EntityFrameworkRepositoryContext(IBus bus) : base(bus) { }

        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj)
        {
            localCtx.Value.Set<TAggregateRoot>().Remove(obj);
            Committed = false;
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot obj)
        {
            localCtx.Value.Entry<TAggregateRoot>(obj).State = System.Data.EntityState.Modified;
            Committed = false;
        }

        public override void RegisterNew<TAggregateRoot>(TAggregateRoot obj)
        {
            localCtx.Value.Set<TAggregateRoot>().Add(obj);
            Committed = false;
        }

        public override void Rollback()
        {
            Committed = false;
        }

        protected override void DoCommit()
        {
            if (!Committed)
            {
                var validationErrors = localCtx.Value.GetValidationErrors();
                var count = localCtx.Value.SaveChanges();
                Committed = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Committed)
                    Commit();
                localCtx.Value.Dispose();
                localCtx.Dispose();
                base.Dispose(disposing);
            }
        }

        #region IEntityFrameworkRepositoryContext Members

        public DbContext Context
        {
            get { return localCtx.Value; }
        }

        #endregion
    }
}
