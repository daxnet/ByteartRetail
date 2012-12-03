using ByteartRetail.Domain.Specifications;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示实现该接口的类型是应用于某种聚合根的仓储类型。
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型。</typeparam>
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        #region Properties
        /// <summary>
        /// 获取当前仓储所使用的仓储上下文实例。
        /// </summary>
        IRepositoryContext Context { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 将指定的聚合根添加到仓储中。
        /// </summary>
        /// <param name="aggregateRoot">需要添加到仓储的聚合根实例。</param>
        void Add(TAggregateRoot aggregateRoot);
        /// <summary>
        /// 根据聚合根的ID值，从仓储中读取聚合根。
        /// </summary>
        /// <param name="key">聚合根的ID值。</param>
        /// <returns>聚合根实例。</returns>
        TAggregateRoot GetByKey(Guid key);
        /// <summary>
        /// 从仓储中读取所有聚合根。
        /// </summary>
        /// <returns>所有的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll();
        /// <summary>
        /// 以指定的排序字段和排序方式，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <returns>排序后的所有聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// 以指定的排序字段和排序方式，以及分页参数，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <returns>带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// 根据指定的规约，从仓储中获取所有符合条件的聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>所有符合条件的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <returns>所有符合条件的、排序后的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以及分页参数，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <returns>符合条件的、排序后的带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// 以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 以指定的排序字段和排序方式，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>排序后的所有聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 以指定的排序字段和排序方式，以及分页参数，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的已经排序的聚合根。</returns>
        IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以及分页参数，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的已经排序的聚合根。</returns>
        PagedResult<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约获取聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>聚合根。</returns>
        TAggregateRoot Get(ISpecification<TAggregateRoot> specification);
        /// <summary>
        /// 根据指定的规约，以饥饿加载的方式获取聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>聚合根。</returns>
        TAggregateRoot Get(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 从仓储中查找所有聚合根。
        /// </summary>
        /// <returns>所有的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll();
        /// <summary>
        /// 以指定的排序字段和排序方式，从仓储中查找所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <returns>排序后的所有聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// 以指定的排序字段和排序方式，以及分页参数，从仓储中查找所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <returns>带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// 根据指定的规约，从仓储中查找所有符合条件的聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>所有符合条件的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <returns>所有符合条件的、排序后的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以及分页参数，从仓储中读取所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <returns>符合条件的、排序后的带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        /// <summary>
        /// 以饥饿加载的方式查找所有聚合根。
        /// </summary>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 以指定的排序字段和排序方式，以饥饿加载的方式获取所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>排序后的所有聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 以指定的排序字段和排序方式，以及分页参数，以饥饿加载的方式查找所有聚合根。
        /// </summary>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>带有分页信息的聚合根集合。</returns>
        PagedResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以饥饿加载的方式查找所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以饥饿加载的方式查找所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的已经排序的聚合根。</returns>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约，以指定的排序字段和排序方式，以及分页参数，以饥饿加载的方式查找所有聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="sortPredicate">用于表述排序字段的Lambda表达式。</param>
        /// <param name="sortOrder">排序方式。</param>
        /// <param name="pageNumber">分页的页码。</param>
        /// <param name="pageSize">分页的页面大小。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>所有符合条件的已经排序的聚合根。</returns>
        PagedResult<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 根据指定的规约查找聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>聚合根。</returns>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification);
        /// <summary>
        /// 根据指定的规约，以饥饿加载的方式查找聚合根。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <param name="eagerLoadingProperties">需要进行饥饿加载的属性Lambda表达式。</param>
        /// <returns>聚合根。</returns>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        /// <summary>
        /// 返回一个<see cref="Boolean"/>值，该值表示符合指定规约条件的聚合根是否存在。
        /// </summary>
        /// <param name="specification">规约。</param>
        /// <returns>如果符合指定规约条件的聚合根存在，则返回true，否则返回false。</returns>
        bool Exists(ISpecification<TAggregateRoot> specification);
        /// <summary>
        /// 将指定的聚合根从仓储中移除。
        /// </summary>
        /// <param name="aggregateRoot">需要从仓储中移除的聚合根。</param>
        void Remove(TAggregateRoot aggregateRoot);
        /// <summary>
        /// 更新指定的聚合根。
        /// </summary>
        /// <param name="aggregateRoot">需要更新的聚合根。</param>
        void Update(TAggregateRoot aggregateRoot);

        #endregion
    }
}
