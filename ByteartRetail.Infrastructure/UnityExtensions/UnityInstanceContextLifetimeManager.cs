using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Unity lifetime manager to support <see cref="System.ServiceModel.InstanceContext"/>.
    /// </summary>
    public class UnityInstanceContextLifetimeManager : UnityWcfLifetimeManager<InstanceContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceContextLifetimeManager"/> class.
        /// </summary>
        public UnityInstanceContextLifetimeManager()
            : base()
        {
        }

        /// <summary>
        /// Returns the appropriate extension for the current lifetime manager.
        /// </summary>
        /// <returns>The registered extension for the current lifetime manager, otherwise, null if the extension is not registered.</returns>
        protected override UnityWcfExtension<InstanceContext> FindExtension()
        {
            return UnityInstanceContextExtension.Current;
        }
    }
}
