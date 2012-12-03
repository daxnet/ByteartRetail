using ByteartRetail.Domain.Model;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示用于“商品分类”聚合根的仓储接口。
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}
