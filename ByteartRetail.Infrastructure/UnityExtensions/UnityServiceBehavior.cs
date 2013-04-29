using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Configures the instance provider to use the <see cref="UnityInstanceProvider"/> for service creation.
    /// </summary>
    public class UnityServiceBehavior : IServiceBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceBehavior"/> class. 
        /// </summary>
        public UnityServiceBehavior()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public string ContainerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="System.ServiceModel.OperationContext"/> support is enabled.
        /// </summary>
        public bool OperationContextEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="System.ServiceModel.InstanceContext"/> support is enabled.
        /// </summary>
        public bool InstanceContextEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="System.ServiceModel.ServiceHostBase"/> support is enabled.
        /// </summary>
        public bool ServiceHostBaseEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="System.ServiceModel.IContextChannel"/> support is enabled.
        /// </summary>
        public bool ContextChannelEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        /// <remarks>Not used in this behavior.</remarks>
        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        /// <remarks>Updates the endpoints instance providers to use the <see cref="UnityInstanceProvider"/>.</remarks>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            if (serviceDescription == null)
            {
                throw new ArgumentNullException("serviceDescription");
            }

            if (serviceHostBase == null)
            {
                throw new ArgumentNullException("serviceHostBase");
            }

            if (this.ServiceHostBaseEnabled)
            {
                serviceHostBase.Extensions.Add(new UnityServiceHostBaseExtension());

                // We need to subscribe to the Closing event so we can remove the extension.
                serviceHostBase.Closing += new System.EventHandler(this.ServiceHostBaseClosing);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.InstanceProvider =
                            new UnityInstanceProvider(serviceDescription.ServiceType, this.ContainerName);

                        if (this.OperationContextEnabled)
                        {
                            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new UnityOperationContextMessageInspector());
                        }

                        if (this.InstanceContextEnabled)
                        {
                            endpointDispatcher.DispatchRuntime.InstanceContextInitializers.Add(new UnityInstanceContextInitializer());
                        }

                        if (this.ContextChannelEnabled)
                        {
                            foreach (DispatchOperation operation in endpointDispatcher.DispatchRuntime.Operations)
                            {
                                operation.CallContextInitializers.Add(new UnityCallContextInitializer());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        /// <remarks>Not used in this behavior.</remarks>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Occurs when a communication object transitions into the closing state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="System.EventArgs" /> that contains no event data.</param>
        private void ServiceHostBaseClosing(object sender, System.EventArgs e)
        {
            ServiceHostBase serviceHostBase = sender as ServiceHostBase;
            if (serviceHostBase != null)
            {
                serviceHostBase.Closing -= new System.EventHandler(this.ServiceHostBaseClosing);

                // We have to get this manually, as the operation context has been disposed by now.
                UnityServiceHostBaseExtension extension = serviceHostBase.Extensions.Find<UnityServiceHostBaseExtension>();
                if (extension != null)
                {
                    serviceHostBase.Extensions.Remove(extension);
                }
            }
        }
    }
}
