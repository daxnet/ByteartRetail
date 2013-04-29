using System;
using System.Globalization;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Abstract base class for Unity WCF lifetime support.
    /// </summary>
    /// <typeparam name="T">IExtensibleObject for which to register Unity lifetime manager support.</typeparam>
    public abstract class UnityWcfLifetimeManager<T> : LifetimeManager
        where T : IExtensibleObject<T>
    {
        /// <summary>
        /// Key for Unity to use for the associated object's instance.
        /// </summary>
        private Guid key = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance of the UnityWcfLifetimeManager class.
        /// </summary>
        protected UnityWcfLifetimeManager()
            : base()
        {
        }

        /// <summary>
        /// Gets the currently registered UnityWcfExtension for this lifetime manager.
        /// </summary>
        private UnityWcfExtension<T> Extension
        {
            get
            {
                UnityWcfExtension<T> extension = this.FindExtension();
                if (extension == null)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentCulture, "UnityWcfExtension<{0}> must be registered in WCF.", typeof(T).Name));
                }

                return extension;
            }
        }

        /// <summary>
        /// Gets the object instance for the given key from the currently registered extension.
        /// </summary>
        /// <returns>The object instance associated with the given key.  If no instance is registered, null is returned.</returns>
        public override object GetValue()
        {
            return this.Extension.FindInstance(this.key);
        }

        /// <summary>
        /// Removes the object instance for the given key from the currently registered extension.
        /// </summary>
        public override void RemoveValue()
        {
            // Not called, but just in case.
            this.Extension.RemoveInstance(this.key);
        }

        /// <summary>
        /// Associates the supplied object instance with the given key in the currently registered extension.
        /// </summary>
        /// <param name="newValue">The object to register in the currently registered extension.</param>
        public override void SetValue(object newValue)
        {
            this.Extension.RegisterInstance(this.key, newValue);
        }

        /// <summary>
        /// Finds the currently registered UnityWcfExtension for this lifetime manager.
        /// </summary>
        /// <returns>Currently registered UnityWcfExtension of the given type, or null if no UnityWcfExtension is registered.</returns>
        protected abstract UnityWcfExtension<T> FindExtension();
    }
}
