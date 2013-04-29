using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Unity lifetime manager to support <see cref="System.ServiceModel.ServiceHostBase"/>.
    /// </summary>
    public class UnityServiceHostBaseLifetimeManager : UnityWcfLifetimeManager<ServiceHostBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostBaseLifetimeManager"/> class.
        /// </summary>
        public UnityServiceHostBaseLifetimeManager()
            : base()
        {
        }

        /// <summary>
        /// Returns the appropriate extension for the current lifetime manager.
        /// </summary>
        /// <returns>The registered extension for the current lifetime manager, otherwise, null if the extension is not registered.</returns>
        protected override UnityWcfExtension<ServiceHostBase> FindExtension()
        {
            return UnityServiceHostBaseExtension.Current;
        }
    }
}
