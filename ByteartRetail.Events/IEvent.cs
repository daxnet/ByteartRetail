using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events
{
    public interface IEvent
    {
        /// <summary>
        /// 获取事件的全局唯一标识。
        /// </summary>
        Guid ID { get; }
        /// <summary>
        /// 获取产生事件的时间戳。
        /// </summary>
        DateTime TimeStamp { get; }
    }
}
