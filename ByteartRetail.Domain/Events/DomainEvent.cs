using ByteartRetail.Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using ByteartRetail.Events;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承于该类的类型为领域事件。
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity source;
        private Guid id = Guid.NewGuid();
        private DateTime timeStamp = DateTime.UtcNow;
        #endregion

        #region Ctor
        public DomainEvent() { }
        public DomainEvent(IEntity source)
        {
            this.source = source;
        }
        #endregion

        #region IDomainEvent Members
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public IEntity Source
        {
            get { return source; }
        }
        #endregion

        #region Public Static Methods

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
            foreach (var handler in handlers)
            {
                if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                    Task.Factory.StartNew(() => handler.Handle(domainEvent));
                else
                    handler.Handle(domainEvent);
            }
        }

        public static void Publish<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = ServiceLocator.Instance.ResolveAll<IDomainEventHandler<TDomainEvent>>();
            if (handlers != null && handlers.Count() > 0)
            {
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        if (handler.GetType().IsDefined(typeof(HandlesAsynchronouslyAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent)));
                        }
                        else
                            handler.Handle(domainEvent);
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
                callback(domainEvent, false, null);
        }
        #endregion
    }
}
