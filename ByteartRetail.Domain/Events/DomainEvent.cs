using System;

namespace ByteartRetail.Domain.Events
{
    /// <summary>
    /// 表示继承于该类的类型为领域事件。
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        #region Private Fields
        private readonly IEntity source;
        private readonly Guid id = Guid.NewGuid();
        private readonly DateTime timeStamp = DateTime.UtcNow;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>DomainEvent</c>类型的实例。
        /// </summary>
        /// <param name="source">产生领域事件的事件源对象。</param>
        public DomainEvent(IEntity source)
        {
            this.source = source;
        }
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

        /// <summary>
        /// 获取产生领域事件的事件源对象。
        /// </summary>
        public IEntity Source
        {
            get { return source; }
        }
        #endregion
    }
}
