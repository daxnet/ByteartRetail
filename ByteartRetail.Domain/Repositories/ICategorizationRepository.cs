using ByteartRetail.Domain.Model;
using ByteartRetail.Infrastructure;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示用于“商品分类关系”聚合根的仓储接口。
    /// </summary>
    public interface ICategorizationRepository : IRepository<Categorization>
    {
        #region Methods
        /// <summary>
        /// 获取指定分类下的所有商品信息。
        /// </summary>
        /// <param name="category">指定的商品分类。</param>
        /// <returns>属于指定分类下的所有商品信息。</returns>
        IEnumerable<Product> GetProductsForCategory(Category category);
        /// <summary>
        /// 以分页的方式，获取指定分类下的所有商品信息。
        /// </summary>
        /// <param name="category">指定的商品分类。</param>
        /// <param name="pageNumber">所请求的分页页码。</param>
        /// <param name="pageSize">所请求的分页大小。</param>
        /// <returns>属于指定分类下的某页的商品信息。</returns>
        PagedResult<Product> GetProductsForCategoryWithPagination(Category category, int pageNumber, int pageSize);
        /// <summary>
        /// 获取商品所属的商品分类。
        /// </summary>
        /// <param name="product">商品信息。</param>
        /// <returns>商品分类。</returns>
        Category GetCategoryForProduct(Product product);
        #endregion
    }
}
