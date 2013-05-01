using ByteartRetail.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    /// <summary>
    /// 表示Product仓储的一个具体实现。
    /// </summary>
    public class ProductRepository : EntityFrameworkRepository<Product>, IProductRepository
    {
        #region Ctor
        public ProductRepository(IRepositoryContext context) : base(context) { }
        #endregion

        #region IProductRepository Members
        /// <summary>
        /// 获取特色商品的列表。
        /// </summary>
        /// <param name="count">需要获取的特色商品的个数。默认值：0，表示获取所有特色商品。</param>
        /// <returns>特色商品列表。</returns>
        public IEnumerable<Product> GetFeaturedProducts(int count = 0)
        {
            var ctx = this.EFContext.Context as ByteartRetailDbContext;
            if (ctx != null)
            {
                var query = from p in ctx.Products
                            where p.IsFeatured
                            select p;
                if (count == 0)
                    return query.ToList();
                else
                    return query.Take(count).ToList();
            }
            return null;
        }

        #endregion
    }
}
