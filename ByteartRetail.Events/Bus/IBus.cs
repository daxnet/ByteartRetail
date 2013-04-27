using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// Represents the message bus.
    /// </summary>
    public interface IBus<TMessage> : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Publishes the specified message to the bus.
        /// </summary>
        /// <param name="message">The message to be published.</param>
        void Publish(TMessage message);
        /// <summary>
        /// Publishes a collection of messages to the bus.
        /// </summary>
        /// <param name="messages">The messages to be published.</param>
        void Publish(IEnumerable<TMessage> messages);
        /// <summary>
        /// Clears the published messages waiting for commit.
        /// </summary>
        void Clear();
    }
}
