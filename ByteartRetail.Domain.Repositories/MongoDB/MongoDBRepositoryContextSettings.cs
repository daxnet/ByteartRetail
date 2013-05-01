using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public class MongoDBRepositoryContextSettings : IMongoDBRepositoryContextSettings
    {
        #region IMongoDBRepositoryContextSettings Members

        public string DatabaseName
        {
            get { return "ByteartRetail"; }
        }

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

        public MongoDatabaseSettings GetDatabaseSettings(MongoServer server)
        {
            return new MongoDatabaseSettings();
        }

        public MapTypeToCollectionNameDelegate MapTypeToCollectionName
        {
            get
            {
                return t =>
                {
                    if (t.Name.Contains("_"))
                        return t.Name.Substring(0, t.Name.IndexOf('_'));
                    return t.Name;
                };
            }
        }

        #endregion
    }
}
