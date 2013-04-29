using ByteartRetail.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 
    /// </summary>
    public class EventBus : DisposableObject, IEventBus
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly ThreadLocal<Queue<object>> messageQueue = new ThreadLocal<Queue<object>>(() => new Queue<object>());
        private readonly IEventAggregator aggregator;
        private ThreadLocal<bool> committed = new ThreadLocal<bool>(() => true);
        private readonly MethodInfo publishMethod;

        public EventBus(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;
            publishMethod = (from m in aggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Publish" &&
                             parameters != null &&
                             parameters.Length == 1
                             select m).First();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                messageQueue.Dispose();
                committed.Dispose();
            }
        }

        #region IBus Members

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent
        {
            messageQueue.Value.Enqueue(message);
            committed.Value = false;
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent
        {
            foreach (var message in messages)
                Publish(message);
        }

        public void Clear()
        {
            messageQueue.Value.Clear();
            committed.Value = true;
        }

        #endregion

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return committed.Value; }
        }

        public void Commit()
        {
            while (messageQueue.Value.Count > 0)
            {
                var evnt = messageQueue.Value.Dequeue();
                var evntType = evnt.GetType();
                var method = publishMethod.MakeGenericMethod(evntType);
                method.Invoke(aggregator, new object[] { evnt });
            }
            committed.Value = true;
        }

        public void Rollback()
        {
            Clear();
        }

        public Guid ID
        {
            get { return id; }
        }

        #endregion
    }
}
