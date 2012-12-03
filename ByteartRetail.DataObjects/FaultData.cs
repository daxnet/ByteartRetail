using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ByteartRetail.DataObjects
{
    /// <summary>
    /// Represents the data to be transferred through the
    /// network which contains the fault exception information.
    /// </summary>
    [DataContract]
    public class FaultData
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the message of the fault data.
        /// </summary>
        [DataMember(Order=0)]
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the full message of the fault data.
        /// </summary>
        [DataMember(Order=1)]
        public string FullMessage { get; set; }
        /// <summary>
        /// Gets or sets the stack trace information of the fault exception.
        /// </summary>
        [DataMember(Order=2)]
        public string StackTrace { get; set; }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Creates a new instance of <c>FaultData</c> class from the specified <see cref="System.Exception"/> object.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> object which carries the error information.</param>
        /// <returns>A new instance of <c>FaultData</c> class.</returns>
        public static FaultData CreateFromException(Exception ex)
        {
            return new FaultData
            {
                Message = ex.Message,
                FullMessage = ex.ToString(),
                StackTrace = ex.StackTrace
            };
        }
        /// <summary>
        /// Creates a new instance of <see cref="FaultReason"/> class from the specified <see cref="Exception"/> object.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> object which carries the error information.</param>
        /// <returns>A new instance of <see cref="FaultReason"/> class.</returns>
        public static FaultReason CreateFaultReason(Exception ex)
        {
            return new FaultReason(ex.Message);
        }
        #endregion
    }
}
