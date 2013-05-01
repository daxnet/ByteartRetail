using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// Represents the methods that maps a given <see cref="Type"/> object to
    /// the name of the <see cref="MongoCollection"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> object.</param>
    /// <returns>The name of the <see cref="MongoCollection"/>.</returns>
    public delegate string MapTypeToCollectionNameDelegate(Type type);

    /// <summary>
    /// Represents that the implemented classes are MongoDB repository context settings.
    /// </summary>
    public interface IMongoDBRepositoryContextSettings
    {
        /// <summary>
        /// Gets the database name.
        /// </summary>
        string DatabaseName { get; }
        /// <summary>
        /// Gets the instance of <see cref="MongoServerSettings"/> class which represents the
        /// settings for MongoDB server.
        /// </summary>
        MongoServerSettings ServerSettings { get; }
        /// <summary>
        /// Gets the instance of <see cref="MongoDatabaseSettings"/> class which represents the
        /// settings for MongoDB database.
        /// </summary>
        /// <param name="server">The MongoDB server instance.</param>
        /// <returns>The instance of <see cref="MongoDatabaseSettings"/> class.</returns>
        MongoDatabaseSettings GetDatabaseSettings(MongoServer server);
        /// <summary>
        /// Gets the method that maps a given <see cref="Type"/> object to
        /// the name of the <see cref="MongoCollection"/>.
        /// </summary>
        MapTypeToCollectionNameDelegate MapTypeToCollectionName { get; }
    }
}
