using ByteartRetail.Events;
using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示领域事件的接口，所有继承于此接口的类型都是一种领域事件。
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 获取产生领域事件的事件源对象。
        /// </summary>
        IEntity Source { get; }
    }
}
