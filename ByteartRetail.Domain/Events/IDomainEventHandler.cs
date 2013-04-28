using ByteartRetail.Events;
using System;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承于该接口的类型是领域事件处理器类型。
    /// </summary>
    /// <typeparam name="TDomainEvent">领域事件处理器所能处理的领域事件的类型。</typeparam>
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
    }
}
