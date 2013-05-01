using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Threading;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    /// <summary>
    /// Represents the MongoDB repository context.
    /// </summary>
    public class MongoDBRepositoryContext : RepositoryContext, IMongoDBRepositoryContext
    {
        #region Private Fields
        private readonly Guid id = Guid.NewGuid();
        private readonly MongoServer server;
        private readonly MongoDatabase database;
        private readonly IMongoDBRepositoryContextSettings settings;
        private readonly object syncObj = new object();
        private readonly Dictionary<Type, MongoCollection> mongoCollections = new Dictionary<Type, MongoCollection>();
        private readonly ThreadLocal<List<object>> localNewCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localModifiedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        private readonly ThreadLocal<List<object>> localDeletedCollection = new ThreadLocal<List<object>>(() => new List<object>());
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>MongoDBRepositoryContext</c> class.
        /// </summary>
        /// <param name="settings">The <see cref="IMongoDBRepositoryContextSettings"/> instance which contains
        /// the setting information for initializing the repository context.</param>
        public MongoDBRepositoryContext(IMongoDBRepositoryContextSettings settings)
        {
            this.settings = settings;
            server = new MongoServer(settings.ServerSettings);
            database = server.GetDatabase(settings.DatabaseName, settings.GetDatabaseSettings(server));
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                server.Disconnect();
            }
            base.Dispose(disposing);
        }

        public override void RegisterNew<TAggregateRoot>(TAggregateRoot obj)
        {
            localNewCollection.Value.Add(obj);
            Committed = false;
        }

        public override void RegisterModified<TAggregateRoot>(TAggregateRoot obj)
        {
            if (localDeletedCollection.Value.Contains(obj))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!localModifiedCollection.Value.Contains(obj) && !localNewCollection.Value.Contains(obj))
                localModifiedCollection.Value.Add(obj);
            Committed = false;
        }

        public override void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj)
        {
            if (localNewCollection.Value.Contains(obj))
            {
                if (localNewCollection.Value.Remove(obj))
                    return;
            }
            bool removedFromModified = localModifiedCollection.Value.Remove(obj);
            bool addedToDeleted = false;
            if (!localDeletedCollection.Value.Contains(obj))
            {
                localDeletedCollection.Value.Add(obj);
                addedToDeleted = true;
            }
            Committed = !(removedFromModified || addedToDeleted);
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Registers the MongoDB Bson serialization conventions.
        /// </summary>
        /// <param name="autoGenerateID">A <see cref="Boolean"/> value which indicates whether
        /// the ID value should be automatically generated when a new document is inserting.</param>
        /// <param name="localDateTime">A <see cref="Boolean"/> value which indicates whether
        /// the local date/time should be used when serializing/deserializing <see cref="DateTime"/> values.</param>
        public static void RegisterConventions(bool autoGenerateID = true, bool localDateTime = true)
        {
            RegisterConventions(autoGenerateID, localDateTime, null);
        }
        /// <summary>
        /// Registers the MongoDB Bson serialization conventions.
        /// </summary>
        /// <param name="autoGenerateID">A <see cref="Boolean"/> value which indicates whether
        /// the ID value should be automatically generated when a new document is inserting.</param>
        /// <param name="localDateTime">A <see cref="Boolean"/> value which indicates whether
        /// the local date/time should be used when serializing/deserializing <see cref="DateTime"/> values.</param>
        /// <param name="additionConventions">Additional conventions that needs to be registered.</param>
        public static void RegisterConventions(bool autoGenerateID, bool localDateTime, IEnumerable<IConvention> additionConventions)
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(new NamedIdMemberConvention("id", "Id", "ID", "iD"));
            if (autoGenerateID)
                conventionPack.Add(new GuidIDGeneratorConvention());
            if (localDateTime)
                conventionPack.Add(new UseLocalDateTimeConvention());
            if (additionConventions != null)
                conventionPack.AddRange(additionConventions);

            ConventionRegistry.Register("DefaultConvention", conventionPack, t => true);
        }
        #endregion

        #region IMongoDBRepositoryContext Members
        /// <summary>
        /// Gets a <see cref="IMongoDBRepositoryContextSettings"/> instance which contains the settings
        /// information used by current context.
        /// </summary>
        public IMongoDBRepositoryContextSettings Settings
        {
            get { return settings; }
        }
        /// <summary>
        /// Gets the <see cref="MongoCollection"/> instance by the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> object.</param>
        /// <returns>The <see cref="MongoCollection"/> instance.</returns>
        public MongoCollection GetCollectionForType(Type type)
        {
            lock (syncObj)
            {
                if (this.mongoCollections.ContainsKey(type))
                    return this.mongoCollections[type];
                else
                {
                    MongoCollection mongoCollection = null;
                    if (settings.MapTypeToCollectionName != null)
                        mongoCollection = this.database.GetCollection(settings.MapTypeToCollectionName(type));
                    else
                        mongoCollection = this.database.GetCollection(type.Name);
                    this.mongoCollections.Add(type, mongoCollection);
                    return mongoCollection;
                }
            }
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public override void Commit()
        {
            this.DoCommit();
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            this.Committed = false;
        }

        #endregion

        protected override void DoCommit()
        {
            lock (syncObj)
            {
                foreach (var newObj in this.localNewCollection.Value)
                {
                    MongoCollection collection = this.GetCollectionForType(newObj.GetType());
                    collection.Insert(newObj);
                }
                foreach (var modifiedObj in this.localModifiedCollection.Value)
                {
                    MongoCollection collection = this.GetCollectionForType(modifiedObj.GetType());
                    collection.Save(modifiedObj);
                }
                foreach (var delObj in this.localDeletedCollection.Value)
                {
                    Type objType = delObj.GetType();
                    PropertyInfo propertyInfo = objType.GetProperty("ID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (propertyInfo == null)
                        throw new InvalidOperationException("Cannot delete an object which doesn't contain an ID property.");
                    Guid id = (Guid)propertyInfo.GetValue(delObj, null);
                    MongoCollection collection = this.GetCollectionForType(objType);
                    IMongoQuery query = Query.EQ("_id", id);
                    collection.Remove(query);
                }
                this.ClearRegistrations();
                this.Committed = true;
            }
        }

        public override bool DistributedTransactionSupported
        {
            get { return false; }
        }
    }
}
