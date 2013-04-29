using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Implements the lifetime manager storage for the <see cref="System.ServiceModel.ServiceHostBase"/> extension.
    /// </summary>
    public class UnityServiceHostBaseExtension : UnityWcfExtension<ServiceHostBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostBaseExtension"/> class.
        /// </summary>
        public UnityServiceHostBaseExtension()
            : base()
        {
        }

        /// <summary>
        /// Gets the <see cref="UnityServiceHostBaseExtension"/> for the current service host.
        /// </summary>
        public static UnityServiceHostBaseExtension Current
        {
            get { return OperationContext.Current.Host.Extensions.Find<UnityServiceHostBaseExtension>(); }
        }
    }
}
