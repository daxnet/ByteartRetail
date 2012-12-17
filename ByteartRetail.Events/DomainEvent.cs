using System;

namespace ByteartRetail.Events
{
    /// <summary>
    /// 表示继承于该类的类型为领域事件。
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly Guid id = Guid.NewGuid();
        private readonly DateTime timeStamp = DateTime.UtcNow;
        #endregion

        #region IDomainEvent Members
        /// <summary>
        /// 获取领域事件的全局唯一标识。
        /// </summary>
        public Guid ID
        {
            get { return id; }
        }
        /// <summary>
        /// 获取产生领域事件的时间戳。
        /// </summary>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        #endregion
    }
}
