using System.ServiceModel;

namespace ByteartRetail.Infrastructure.UnityExtensions
{
    /// <summary>
    /// Implements the lifetime manager storage for the <see cref="System.ServiceModel.OperationContext"/> extension.
    /// </summary>
    public class UnityOperationContextExtension : UnityWcfExtension<OperationContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityOperationContextExtension"/> class.
        /// </summary>
        public UnityOperationContextExtension()
            : base()
        {
        }

        /// <summary>
        /// Gets the <see cref="UnityOperationContextExtension"/> for the current operation context.
        /// </summary>
        public static UnityOperationContextExtension Current
        {
            get { return OperationContext.Current.Extensions.Find<UnityOperationContextExtension>(); }
        }
    }
}
