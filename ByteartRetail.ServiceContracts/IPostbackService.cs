using ByteartRetail.DataObjects;
using ByteartRetail.Infrastructure;
using System.ServiceModel;

namespace ByteartRetail.ServiceContracts
{
    /// <summary>
    /// 表示与“服务器信息回发”相关的应用层服务契约。
    /// </summary>
    [ServiceContract(Namespace = "http://www.ByteartRetail.com")]
    public interface IPostbackService : IApplicationServiceContract
    {
        #region Methods
        /// <summary>
        /// 将服务器相关的信息回发给客户端。
        /// </summary>
        /// <returns>服务器相关信息。</returns>
        /// <remarks>此服务仅用于测试，没有实际业务含义。</remarks>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        PostbackDataObject GetPostback();
        #endregion
    }
}
