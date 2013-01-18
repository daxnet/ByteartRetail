using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Infrastructure.Caching
{
    public class AppfabricCacheProvider : ICacheProvider
    {
        private readonly DataCacheFactory factory = new DataCacheFactory();
        private readonly DataCache cache;

        public AppfabricCacheProvider()
        {
            cache = factory.GetDefaultCache();
        }

        #region ICacheProvider Members

        public void Add(string key, string valKey, object value)
        {
            Dictionary<string, object> val = (Dictionary<string, object>)cache.Get(key);
            if (val == null)
            {
                val = new Dictionary<string, object>();
                val.Add(valKey, value);
                cache.Add(key, val);
            }
            else
            {
                if (!val.ContainsKey(valKey))
                    val.Add(valKey, value);
                else
                    val[valKey] = value;
                cache.Put(key, val);
            }
        }

        public void Put(string key, string valKey, object value)
        {
            Add(key, valKey, value);
        }

        public object Get(string key, string valKey)
        {
            if (Exists(key, valKey))
            {
                return ((Dictionary<string, object>)cache.Get(key))[valKey];
            }
            return null;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return cache.Get(key) != null;
        }

        public bool Exists(string key, string valKey)
        {
            var val = cache.Get(key);
            if (val == null)
                return false;
            return ((Dictionary<string, object>)val).ContainsKey(valKey);
        }

        #endregion
    }
}
