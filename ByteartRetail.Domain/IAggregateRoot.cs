
using ByteartRetail.Domain.Events;
using System.Collections.Generic;

namespace ByteartRetail.Domain
{
    /// <summary>
    /// 表示继承于该接口的类型是聚合根类型。
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
        IEnumerable<IDomainEvent> UncommittedEvents { get; }
        void ClearEvents();
    }
}
