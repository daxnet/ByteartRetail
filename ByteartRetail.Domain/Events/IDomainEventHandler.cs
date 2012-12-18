using ByteartRetail.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
    }
}
