using System;
using System.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteartRetail.Infrastructure;

namespace ByteartRetail.Events.Bus
{
    public class MSMQEventBus : DisposableObject, IEventBus
    {
        #region Private Fields
        private readonly Guid id = Guid.NewGuid();
        private volatile bool committed = true;
        private readonly bool useInternalTransaction;
        private readonly MessageQueue messageQueue;
        private readonly object lockObj = new object();
        private readonly MSMQBusOptions options;
        private readonly Queue<object> mockQueue = new Queue<object>();
        #endregion

        public MSMQEventBus(string path)
        {
            this.options = new MSMQBusOptions(path);
            this.messageQueue = new MessageQueue(path,
                options.SharedModeDenyReceive,
                options.EnableCache, options.QueueAccessMode);
            this.messageQueue.Formatter = options.MessageFormatter;
            this.useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        public MSMQEventBus(string path, bool useInternalTransaction)
        {
            this.options = new MSMQBusOptions(path, useInternalTransaction);
            this.messageQueue = new MessageQueue(path,
                options.SharedModeDenyReceive,
                options.EnableCache, options.QueueAccessMode);
            this.messageQueue.Formatter = options.MessageFormatter;
            this.useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        public MSMQEventBus(MSMQBusOptions options)
        {
            this.options = options;
            this.messageQueue = new MessageQueue(options.Path,
                options.SharedModeDenyReceive,
                options.EnableCache, options.QueueAccessMode);
            this.messageQueue.Formatter = options.MessageFormatter;
            this.useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        private void SendMessage<TMessage>(TMessage message, MessageQueueTransaction transaction = null)
            where TMessage : class, IEvent
        {
            Message msmqMessage = new Message(message);
            if (useInternalTransaction)
            {
                if (transaction == null)
                    throw new ArgumentNullException("transaction");

                messageQueue.Send(msmqMessage, transaction);
            }
            else
            {
                messageQueue.Send(msmqMessage, MessageQueueTransactionType.Automatic);
            }
        }

        private void SendMessage(object message, MessageQueueTransaction transaction = null)
        {
            var sendMessageMethod = (from m in this.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                                     let methodName = m.Name
                                     let generic = m.IsGenericMethod
                                     where generic &&
                                     methodName == "SendMessage"
                                     select m).First();
            var evntType = message.GetType();
            sendMessageMethod.MakeGenericMethod(evntType).Invoke(this, new object[] { message, transaction });
        }

        #region IBus Members

        public void Publish<TMessage>(TMessage message) where TMessage : class, IEvent
        {
            lock (lockObj)
            {
                mockQueue.Enqueue(message);
                committed = false;
            }
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : class, IEvent
        {
            lock (lockObj)
            {
                messages.ToList().ForEach(p => { mockQueue.Enqueue(p); committed = false; });
            }
        }

        public void Clear()
        {
            lock (lockObj)
            {
                this.mockQueue.Clear();
            }
        }

        public Guid ID
        {
            get { return id; }
        }

        #endregion

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public bool Committed
        {
            get { return this.committed; }
        }

        public void Commit()
        {
            lock (lockObj)
            {
                if (this.useInternalTransaction)
                {
                    //backupMessageArray = new TMessage[mockQueue.Count];
                    //mockQueue.CopyTo(backupMessageArray, 0);
                    using (MessageQueueTransaction transaction = new MessageQueueTransaction())
                    {
                        try
                        {
                            transaction.Begin();
                            while (mockQueue.Count > 0)
                            {
                                object msg = mockQueue.Dequeue();
                                SendMessage(msg, transaction);
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Abort();
                            throw;
                        }
                    }
                }
                else
                {
                    while (mockQueue.Count > 0)
                    {
                        object msg = mockQueue.Dequeue();
                        SendMessage(msg);
                    }
                }
                committed = true;
            }
        }

        public void Rollback()
        {
            committed = false;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (messageQueue != null)
                {
                    messageQueue.Close();
                    messageQueue.Dispose();
                }
            }
        }
    }
}
