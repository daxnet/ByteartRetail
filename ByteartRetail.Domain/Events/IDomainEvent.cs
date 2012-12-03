using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示领域事件的接口，所有继承于此接口的类型都是一种领域事件。
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        DateTime TimeStamp { get; }
    }
}
