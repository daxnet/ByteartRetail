using ByteartRetail.Events.Handlers;
using System;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承于该接口的类型是领域事件处理器类型。
    /// </summary>
    /// <typeparam name="TDomainEvent">领域事件处理器所能处理的领域事件的类型。</typeparam>
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>, IEquatable<IDomainEventHandler<TDomainEvent>>
        where TDomainEvent : class, IDomainEvent
    {
        /// <summary>
        /// 以异步方式处理领域事件。
        /// </summary>
        /// <param name="evnt">需要处理的领域事件。</param>
        /// <returns>对领域事件进行处理的任务句柄。</returns>
        Task HandleAsync(TDomainEvent evnt);
    }
}
