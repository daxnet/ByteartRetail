using System;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承该抽象类型的类是领域事件处理器。
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        #region IEventHandler<TEvent> Members
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        public abstract void Handle(TEvent evnt);
        /// <summary>
        /// 以异步方式处理领域事件。
        /// </summary>
        /// <param name="evnt">需要处理的领域事件。</param>
        /// <returns>对领域事件进行处理的任务句柄。</returns>
        public async Task HandleAsync(TEvent evnt)
        {
            await Task.Factory.StartNew(() => Handle(evnt));
        }
        #endregion

        #region IEquatable<IDomainEventHandler<TEvent>> Members
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前对象是否与给定的类型相同的另一对象相等。
        /// </summary>
        /// <param name="other">需要比较的与当前对象类型相同的另一对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public virtual bool Equals(IDomainEventHandler<TEvent> other)
        {
            return Equals((object)other);
        }

        #endregion
    }
}
