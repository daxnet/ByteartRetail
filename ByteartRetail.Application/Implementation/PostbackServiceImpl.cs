using ByteartRetail.DataObjects;
using ByteartRetail.Domain.Repositories;
using ByteartRetail.ServiceContracts;
using System;

namespace ByteartRetail.Application.Implementation
{
    /// <summary>
    /// 表示与“服务器信息回发”相关的一种服务实现。
    /// </summary>
    public class PostbackServiceImpl : ApplicationService, IPostbackService
    {
        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>PostbackServiceImpl</c>实例。
        /// </summary>
        /// <param name="context">用来初始化<c>PostbackServiceImpl</c>类型的仓储上下文实例。</param>
        public PostbackServiceImpl(IRepositoryContext context) : base(context) { }
        #endregion

        #region IPostbackService Members
        /// <summary>
        /// 创建商品信息。
        /// </summary>
        /// <param name="productDataObjects">需要创建的商品信息。</param>
        /// <returns>已创建的商品信息。</returns>
        public PostbackDataObject GetPostback()
        {
            PostbackDataObject result = new PostbackDataObject
            {
                ID = Guid.NewGuid().ToString(),
                ServerArchitecture = string.Format("{0} processors on {1} bit OS.", Environment.ProcessorCount, Environment.Is64BitOperatingSystem ? "64" : "32"),
                ServerDateTime = DateTime.Now,
                ServerOS = Environment.OSVersion.ToString(),
                MachineName = Environment.MachineName,
                CLRVersion = Environment.Version.ToString()
            };

            return result;
        }

        #endregion
    }
}
