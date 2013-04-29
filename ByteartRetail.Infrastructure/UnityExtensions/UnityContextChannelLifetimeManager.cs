using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Unity lifetime manager to support <see cref="System.ServiceModel.IContextChannel"/>.
    /// </summary>
    public class UnityContextChannelLifetimeManager : UnityWcfLifetimeManager<IContextChannel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityContextChannelLifetimeManager"/> class.
        /// </summary>
        public UnityContextChannelLifetimeManager()
            : base()
        {
        }

        /// <summary>
        /// Returns the appropriate extension for the current lifetime manager.
        /// </summary>
        /// <returns>The registered extension for the current lifetime manager, otherwise, null if the extension is not registered.</returns>
        protected override UnityWcfExtension<IContextChannel> FindExtension()
        {
            return UnityContextChannelExtension.Current;
        }
    }
}
