using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Implements the lifetime manager storage for the <see cref="System.ServiceModel.IContextChannel"/> extension.
    /// </summary>
    public class UnityContextChannelExtension : UnityWcfExtension<IContextChannel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityContextChannelExtension"/> class.
        /// </summary>
        public UnityContextChannelExtension()
            : base()
        {
        }

        /// <summary>
        /// Gets the <see cref="UnityContextChannelExtension"/> for the current channel.
        /// </summary>
        public static UnityContextChannelExtension Current
        {
            get { return OperationContext.Current.Channel.Extensions.Find<UnityContextChannelExtension>(); }
        }
    }
}
