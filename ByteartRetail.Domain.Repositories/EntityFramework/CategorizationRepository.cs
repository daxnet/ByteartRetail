using ByteartRetail.Domain.Model;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    public class CategorizationRepository : EntityFrameworkRepository<Categorization>, ICategorizationRepository
    {
        public CategorizationRepository(IRepositoryContext context) : base(context) { }

        #region ICategorizationRepository Members

        public IEnumerable<Product> GetProductsForCategory(Category category)
        {
            var context = EFContext.Context as ByteartRetailDbContext;
            if (context != null)
            {
                var query = from product in context.Products
                            from categorization in context.Categorizations
                            where product.ID == categorization.ProductID &&
                                categorization.CategoryID == category.ID
                            select product;
                return query.ToList();
            }
            throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效。");
        }

        public PagedResult<Product> GetProductsForCategoryWithPagination(Category category, int pageNumber, int pageSize)
        {
            var context = EFContext.Context as ByteartRetailDbContext;
            if (context != null)
            {
                int skip = (pageNumber - 1) * pageSize;
                int take = pageSize;
                var query = from product in context.Products
                            from categorization in context.Categorizations
                            where product.ID == categorization.ProductID &&
                                categorization.CategoryID == category.ID
                            orderby product.Name ascending
                            select product;
                var pagedQuery = query.Skip(skip).Take(take).GroupBy(p => new { Total = query.Count() }).FirstOrDefault();
                if (pagedQuery == null)
                    return null;
                return new PagedResult<Product>(pagedQuery.Key.Total, (pagedQuery.Key.Total + pageSize - 1) / pageSize, pageSize, pageNumber, pagedQuery.Select(p => p).ToList());
            }
            throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效。");
        }

        public Category GetCategoryForProduct(Product product)
        {
            var context = EFContext.Context as ByteartRetailDbContext;
            if (context != null)
            {
                var query = from category in context.Categories
                            from categorization in context.Categorizations
                            where categorization.ProductID == product.ID &&
                            categorization.CategoryID == category.ID
                            select category;
                return query.FirstOrDefault();
            }
            throw new InvalidOperationException("指定的仓储上下文（Repository Context）无效。");
        }
        #endregion
    }
}
