using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace ByteartRetail.Infrastructure
{
    class ContainerExtension : IExtension<OperationContext>
    {
        #region Members

        public object Value { get; set; }

        #endregion

        #region IExtension<OperationContext> Members

        public void Attach(OperationContext owner)
        {

        }

        public void Detach(OperationContext owner)
        {

        }

        #endregion
    }

    public class WcfPerRequestLifetimeManager : LifetimeManager
    {
        #region Private Fields
        private readonly Guid key = Guid.NewGuid();
        #endregion

        public WcfPerRequestLifetimeManager() : this(Guid.NewGuid()) { }

        WcfPerRequestLifetimeManager(Guid key)
        {
            if (key == Guid.Empty)
                throw new ArgumentException("Key is empty.");

            this.key = key;
        }

        #region Public Methods
        /// <summary>
        /// Retrieve a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <returns>The object desired, or null if no such object is currently stored.</returns>
        public override object GetValue()
        {
            object result = null;

            //Get object depending on  execution environment ( WCF without HttpContext,HttpContext or CallContext)

            if (OperationContext.Current != null)
            {
                //WCF without HttpContext environment
                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
                if (containerExtension != null)
                {
                    result = containerExtension.Value;
                }
            }
            else if (HttpContext.Current != null)
            {
                //HttpContext avaiable ( ASP.NET ..)
                if (HttpContext.Current.Items[key.ToString()] != null)
                    result = HttpContext.Current.Items[key.ToString()];
            }
            else
            {
                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
                result = CallContext.GetData(key.ToString());
            }

            return result;
        }
        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        public override void RemoveValue()
        {
            if (OperationContext.Current != null)
            {
                //WCF without HttpContext environment
                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
                if (containerExtension != null)
                    OperationContext.Current.Extensions.Remove(containerExtension);

            }
            else if (HttpContext.Current != null)
            {
                //HttpContext avaiable ( ASP.NET ..)
                if (HttpContext.Current.Items[key.ToString()] != null)
                    HttpContext.Current.Items[key.ToString()] = null;
            }
            else
            {
                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
                CallContext.FreeNamedDataSlot(key.ToString());
            }
        }
        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        public override void SetValue(object newValue)
        {
            if (OperationContext.Current != null)
            {
                //WCF without HttpContext environment
                ContainerExtension containerExtension = OperationContext.Current.Extensions.Find<ContainerExtension>();
                if (containerExtension == null)
                {
                    containerExtension = new ContainerExtension()
                    {
                        Value = newValue
                    };

                    OperationContext.Current.Extensions.Add(containerExtension);
                }
            }
            else if (HttpContext.Current != null)
            {
                //HttpContext avaiable ( ASP.NET ..)
                if (HttpContext.Current.Items[key.ToString()] == null)
                    HttpContext.Current.Items[key.ToString()] = newValue;
            }
            else
            {
                //Not in WCF or ASP.NET Environment, UnitTesting, WinForms, WPF etc.
                CallContext.SetData(key.ToString(), newValue);
            }
        }
        #endregion
    }
}
