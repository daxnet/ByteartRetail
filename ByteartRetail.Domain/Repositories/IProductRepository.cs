using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示用于“商品”聚合根的仓储接口。
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        #region Methods
        /// <summary>
        /// 获取特色商品的列表。
        /// </summary>
        /// <param name="count">需要获取的特色商品的个数。默认值：0，表示获取所有特色商品。</param>
        /// <returns>特色商品列表。</returns>
        IEnumerable<Product> GetFeaturedProducts(int count = 0);
        #endregion
    }
}
