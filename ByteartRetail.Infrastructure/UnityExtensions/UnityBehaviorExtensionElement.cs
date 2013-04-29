using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Represents a configuration element that contains a behavior extension 
    /// which enable the user to customize service or endpoint behaviors to include
    /// the container to use when using the <see cref="UnityServiceBehavior"/>.
    /// </summary>
    public class UnityBehaviorExtensionElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Name of the configuration attribute for the container name.
        /// </summary>
        private const string ContainerConfigurationPropertyName = "containerName";

        /// <summary>
        /// Name of the configuration attribute for enabling the OperationContext lifetime manager.
        /// </summary>
        private const string OperationContextEnabledPropertyName = "operationContextEnabled";

        /// <summary>
        /// Name of the configuration attribute for enabling the InstanceContext lifetime manager.
        /// </summary>
        private const string InstanceContextEnabledPropertyName = "instanceContextEnabled";

        /// <summary>
        /// Name of the configuration attribute for enabling the ServiceHostBase lifetime manager.
        /// </summary>
        private const string ServiceHostBaseEnabledPropertyName = "serviceHostBaseEnabled";

        /// <summary>
        /// Name of the configuration attribute for enabling the IContextChannel lifetime manager.
        /// </summary>
        private const string ContextChannelEnabledPropertyName = "contextChannelEnabled";

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <returns>
        /// A <see cref="UnityServiceBehavior"/> type.
        /// </returns>
        public override Type BehaviorType
        {
            get { return typeof(UnityServiceBehavior); }
        }

        /// <summary>
        /// Gets or sets the container name in configuration to use when creating services.
        /// </summary>
        /// <value>The container name in configuration to use when creating services.</value>
        [ConfigurationProperty(ContainerConfigurationPropertyName, IsRequired = false)]
        public string ContainerName
        {
            get { return (string)base[ContainerConfigurationPropertyName]; }
            set { base[ContainerConfigurationPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="UnityOperationContextLifetimeManager"/> support is enabled. 
        /// </summary>
        /// <value>true to enable Unity lifetime manager support for the WCF OperationContext extension, otherwise, false.</value>
        [ConfigurationProperty(OperationContextEnabledPropertyName, IsRequired = false, DefaultValue = false)]
        public bool OperationContextEnabled
        {
            get { return (bool)base[OperationContextEnabledPropertyName]; }
            set { base[OperationContextEnabledPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="UnityInstanceContextLifetimeManager"/> support is enabled. 
        /// </summary>
        /// <value>true to enable Unity lifetime manager support for the WCF InstanceContext extension, otherwise, false.</value>
        [ConfigurationProperty(InstanceContextEnabledPropertyName, IsRequired = false, DefaultValue = false)]
        public bool InstanceContextEnabled
        {
            get { return (bool)base[InstanceContextEnabledPropertyName]; }
            set { base[InstanceContextEnabledPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="UnityServiceHostBaseLifetimeManager"/> support is enabled. 
        /// </summary>
        /// <value>true to enable Unity lifetime manager support for the WCF ServiceHostBase extension, otherwise, false.</value>
        [ConfigurationProperty(ServiceHostBaseEnabledPropertyName, IsRequired = false, DefaultValue = false)]
        public bool ServiceHostBaseEnabled
        {
            get { return (bool)base[ServiceHostBaseEnabledPropertyName]; }
            set { base[ServiceHostBaseEnabledPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="UnityContextChannelLifetimeManager"/> support is enabled. 
        /// </summary>
        /// <value>true to enable Unity lifetime manager support for the WCF IContextChannel extension, otherwise, false.</value>
        [ConfigurationProperty(ContextChannelEnabledPropertyName, IsRequired = false, DefaultValue = false)]
        public bool ContextChannelEnabled
        {
            get { return (bool)base[ContextChannelEnabledPropertyName]; }
            set { base[ContextChannelEnabledPropertyName] = value; }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        /// The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new UnityServiceBehavior()
            {
                ContainerName = this.ContainerName,
                ContextChannelEnabled = this.ContextChannelEnabled,
                InstanceContextEnabled = this.InstanceContextEnabled,
                OperationContextEnabled = this.OperationContextEnabled,
                ServiceHostBaseEnabled = this.ServiceHostBaseEnabled
            };
        }
    }
}
