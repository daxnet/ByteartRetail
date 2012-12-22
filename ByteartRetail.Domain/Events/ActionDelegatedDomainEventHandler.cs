using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示代理给定的领域事件处理委托的领域事件处理器。
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    internal sealed class ActionDelegatedDomainEventHandler<TEvent> : DomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        #region Private Fields
        private readonly Action<TEvent> eventHandlerDelegate;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>ActionDelegatedDomainEventHandler{TEvent}</c>实例。
        /// </summary>
        /// <param name="eventHandlerDelegate">用于当前领域事件处理器所代理的事件处理委托。</param>
        public ActionDelegatedDomainEventHandler(Action<TEvent> eventHandlerDelegate)
        {
            this.eventHandlerDelegate = eventHandlerDelegate;
        }
        #endregion

        #region IEventHandler<TEvent> Members
        /// <summary>
        /// 处理给定的事件。
        /// </summary>
        /// <param name="evnt">需要处理的事件。</param>
        public override void Handle(TEvent evnt)
        {
            this.eventHandlerDelegate(evnt);
        }

        #endregion

        #region IEquatable<ActionDelegatedDomainEventHandler<TEvent>> Members
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前对象是否与给定的类型相同的另一对象相等。
        /// </summary>
        /// <param name="other">需要比较的与当前对象类型相同的另一对象。</param>
        /// <returns>如果两者相等，则返回true，否则返回false。</returns>
        public override bool Equals(IDomainEventHandler<TEvent> other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if ((object)other == (object)null)
                return false;
            ActionDelegatedDomainEventHandler<TEvent> otherDelegate = other as ActionDelegatedDomainEventHandler<TEvent>;
            if ((object)otherDelegate == (object)null)
                return false;
            // 使用Delegate.Equals方法判定两个委托是否是代理的同一方法。
            return Delegate.Equals(this.eventHandlerDelegate, otherDelegate.eventHandlerDelegate);
        }

        #endregion
    }
}
