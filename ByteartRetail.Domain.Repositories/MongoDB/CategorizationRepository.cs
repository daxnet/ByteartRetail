using ByteartRetail.Domain.Model;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class CategorizationRepository : MongoDBRepository<Categorization>, ICategorizationRepository
    {
        public CategorizationRepository(IRepositoryContext context) : base(context) { }

        #region ICategorizationRepository Members

        public IEnumerable<Product> GetProductsForCategory(Category category)
        {
            var categorizationCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Categorization));
            var categorizations = categorizationCollection.AsQueryable<Categorization>().Where(c => c.CategoryID == category.ID).ToList();
            var productCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Product));
            var productsQuery = productCollection.AsQueryable<Product>();
            List<Product> totalList = new List<Product>();
            foreach (var categorization in categorizations)
            {
                totalList.AddRange(productsQuery.Where(p => p.ID == categorization.ProductID).ToList());
            }
            return totalList;
        }

        public PagedResult<Product> GetProductsForCategoryWithPagination(Category category, int pageNumber, int pageSize)
        {
            var categorizationCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Categorization));
            var categorizations = categorizationCollection.AsQueryable<Categorization>().Where(c => c.CategoryID == category.ID).ToList();
            var productCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Product));
            var productsQuery = productCollection.AsQueryable<Product>();
            List<Product> totalList = new List<Product>();
            foreach (var categorization in categorizations)
            {
                totalList.AddRange(productsQuery.Where(p => p.ID == categorization.ProductID).ToList());
            }
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            int count = totalList.Count();
            return new PagedResult<Product>(count, (count + pageSize - 1) / pageSize, pageSize, pageNumber, totalList.Skip(skip).Take(take).ToList());
        }

        public Category GetCategoryForProduct(Product product)
        {
            var categorizationCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Categorization));
            var categorizations = categorizationCollection.AsQueryable<Categorization>().Where(c => c.ProductID == product.ID).FirstOrDefault();
            if (categorizations == null)
                return null;
            var categoryCollection = MongoDBRepositoryContext.GetCollectionForType(typeof(Category));
            return categoryCollection.AsQueryable<Category>().Where(c => c.ID == categorizations.CategoryID).FirstOrDefault();
        }
        #endregion
    }
}
