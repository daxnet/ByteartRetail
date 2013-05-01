using ByteartRetail.Domain.Specifications;
using ByteartRetail.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// Represents the MongoDB repository.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of the aggregate root.</typeparam>
    public class MongoDBRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        #region Private Fields
        private readonly IMongoDBRepositoryContext mongoDBRepositoryContext;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>MongoDBRepository[TAggregateRoot]</c> class.
        /// </summary>
        /// <param name="context">The <see cref="IRepositoryContext"/> object for initializing the current repository.</param>
        public MongoDBRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IMongoDBRepositoryContext)
                mongoDBRepositoryContext = context as MongoDBRepositoryContext;
            else
                throw new InvalidOperationException("Invalid repository context type.");
        }
        #endregion

        internal IMongoDBRepositoryContext MongoDBRepositoryContext
        {
            get { return mongoDBRepositoryContext; }
        }

        #region Protected Methods
        /// <summary>
        /// Adds an aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be added to the repository.</param>
        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            mongoDBRepositoryContext.RegisterNew(aggregateRoot);
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        
        protected override IEnumerable<TAggregateRoot> DoGetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder);
            if (results == null || results.Count() == 0)
                throw new Exception("Cannot get.");
            return results;
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>All the aggregate roots that match the given specification and were sorted by using the given sort predicate and the sort order.</returns>
        
        protected override PagedResult<TAggregateRoot> DoGetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
            if (results == null || results.Count() == 0)
                throw new Exception("Cannot get.");
            return results;
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate roots.</returns>
        
        protected override IEnumerable<TAggregateRoot> DoGetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new Exception("Cannot get.");
            return results;
        }
        /// <summary>
        /// Gets all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate roots.</returns>
        
        protected override PagedResult<TAggregateRoot> DoGetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new Exception("Cannot get.");
            return results;
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <returns>The aggregate roots.</returns>
        protected override IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            var query = collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression());
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        return query.OrderBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return query.OrderByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return query.ToList();
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The number of objects per page.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <returns>The aggregate roots.</returns>
        protected override PagedResult<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            if (sortPredicate == null)
                throw new ArgumentNullException("sortPredicate");

            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            var query = collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression());
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int totalCount = query.Count();
            int totalPages = (totalCount + pageSize - 1) / pageSize;
            if (sortPredicate != null)
            {
                switch (sortOrder)
                {
                    case SortOrder.Ascending:
                        var pagedCollectionAscending = query.OrderBy(sortPredicate).Skip(skip).Take(take).ToList();
                        return new PagedResult<TAggregateRoot>(totalCount, totalPages, pageSize, pageNumber, pagedCollectionAscending);
                    case SortOrder.Descending:
                        var pagedCollectionDescending = query.OrderByDescending(sortPredicate).Skip(skip).Take(take).ToList();
                        return new PagedResult<TAggregateRoot>(totalCount, totalPages, pageSize, pageNumber, pagedCollectionDescending);
                    default:
                        break;
                }
            }
            return null;
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected override IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }
        /// <summary>
        /// Finds all the aggregate roots from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate roots should match.</param>
        /// <param name="sortPredicate">The sort predicate which is used for sorting.</param>
        /// <param name="sortOrder">The <see cref="Apworks.Storage.SortOrder"/> enumeration which specifies the sort order.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected override PagedResult<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }
        /// <summary>
        /// Gets a single aggregate root from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The aggregate root.</returns>
        
        protected override TAggregateRoot DoGet(ISpecification<TAggregateRoot> specification)
        {
            var result = this.DoFind(specification);
            if (result == null)
                throw new Exception("Cannot get.");
            return result;
        }
        /// <summary>
        /// Gets a single aggregate root from repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        
        protected override TAggregateRoot DoGet(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            var result = this.DoFind(specification, eagerLoadingProperties);
            if (result == null)
                throw new Exception("Cannot get.");
            return result;
        }
        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>The instance of the aggregate root.</returns>
        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification)
        {
            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            return collection.AsQueryable<TAggregateRoot>().Where(specification.GetExpression()).FirstOrDefault();
        }
        /// <summary>
        /// Finds a single aggregate root from the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <param name="eagerLoadingProperties">The properties for the aggregated objects that need to be loaded.</param>
        /// <returns>The aggregate root.</returns>
        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties)
        {
            return this.DoFind(specification);
        }
        /// <summary>
        /// Checkes whether the aggregate root, which matches the given specification, exists in the repository.
        /// </summary>
        /// <param name="specification">The specification with which the aggregate root should match.</param>
        /// <returns>True if the aggregate root exists, otherwise false.</returns>
        protected override bool DoExists(ISpecification<TAggregateRoot> specification)
        {
            return this.DoFind(specification) != null;
        }
        /// <summary>
        /// Removes the aggregate root from current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be removed.</param>
        protected override void DoRemove(TAggregateRoot aggregateRoot)
        {
            mongoDBRepositoryContext.RegisterDeleted(aggregateRoot);
        }
        /// <summary>
        /// Updates the aggregate root in the current repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root to be updated.</param>
        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            mongoDBRepositoryContext.RegisterModified(aggregateRoot);
        }
        #endregion

        protected override TAggregateRoot DoGetByKey(Guid key)
        {
            MongoCollection collection = mongoDBRepositoryContext.GetCollectionForType(typeof(TAggregateRoot));
            Guid id = (Guid)key;
            return collection.AsQueryable<TAggregateRoot>().Where(p => p.ID == id).First();
        }
    }
}
