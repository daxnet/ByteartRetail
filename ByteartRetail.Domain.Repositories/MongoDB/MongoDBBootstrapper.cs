using ByteartRetail.Domain.Model;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Repositories.MongoDB
{
    public static class MongoDBBootstrapper
    {
        public static void Bootstrap()
        {
            MongoDBRepositoryContext.RegisterConventions();

            BsonClassMap.RegisterClassMap<ShoppingCart>(s =>
                {
                    s.AutoMap();
                    s.SetIgnoreExtraElements(true);
                });

            BsonClassMap.RegisterClassMap<SalesOrder>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<User>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<UserRole>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Category>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Categorization>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<Product>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<ShoppingCartItem>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<SalesLine>(s =>
            {
                s.AutoMap();
                s.SetIgnoreExtraElements(true);
                s.UnmapProperty<SalesOrder>(p => p.SalesOrder); // bypass circular reference.
            });

        }
    }
}
