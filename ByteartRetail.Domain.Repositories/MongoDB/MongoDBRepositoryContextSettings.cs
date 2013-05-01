using MongoDB.Driver;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// 表示基于MongoDB实现的仓储上下文的配置信息。
    /// </summary>
    /// <remarks>
    /// 如果您的MongoDB配置不是默认的，您可以在此处修改MongoDB的配置信息，比如
    /// 可以在ServerSettings里指定MongoDB的端口号等。
    /// </remarks>
    public class MongoDBRepositoryContextSettings : IMongoDBRepositoryContextSettings
    {
        #region IMongoDBRepositoryContextSettings Members
        /// <summary>
        /// 获取数据库名称。
        /// </summary>
        public string DatabaseName
        {
            get { return "ByteartRetail"; }
        }
        /// <summary>
        /// 获取MongoDB的服务器配置信息。
        /// </summary>
        public MongoServerSettings ServerSettings
        {
            get
            {
                var settings = new MongoServerSettings();
                settings.Server = new MongoServerAddress("localhost");
                settings.WriteConcern = WriteConcern.Acknowledged;
                return settings;
            }
        }
        /// <summary>
        /// 获取数据库配置信息。
        /// </summary>
        /// <param name="server">需要配置的数据库实例。</param>
        /// <returns>数据库配置信息。</returns>
        public MongoDatabaseSettings GetDatabaseSettings(MongoServer server)
        {
            // 您无需做过多的更改：此处仅返回新建的MongoDatabaseSettings实例即可。
            return new MongoDatabaseSettings();
        }
        /// <summary>
        /// 获取用于根据聚合根类型返回其所对应的集合名称的委托实例。
        /// </summary>
        public MapTypeToCollectionNameDelegate MapTypeToCollectionName
        {
            // 此处直接返回null，表示直接使用聚合根的类型名作为集合名称，比如：SalesOrder聚合根
            // 所对应的集合就是SalesOrder。如果你打算自定义集合名称，则请在此处返回一个将聚合根
            // 类型映射为集合名称的委托。
            get { return null; }
        }

        #endregion
    }
}
