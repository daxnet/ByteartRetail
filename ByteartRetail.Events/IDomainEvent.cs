using System;

namespace ByteartRetail.Events
{
    /// <summary>
    /// 表示领域事件的接口，所有继承于此接口的类型都是一种领域事件。
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        
    }
}
