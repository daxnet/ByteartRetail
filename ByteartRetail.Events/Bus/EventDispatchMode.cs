
namespace ByteartRetail.Events.Bus
{
    /// <summary>
    /// 表示事件派发的方式。
    /// </summary>
    public enum EventDispatchMode
    {
        /// <summary>
        /// 表示需要将事件以顺序的方式派发到每个已经注册的事件处理器上进行处理。
        /// </summary>
        Sequential,
        /// <summary>
        /// 表示需要将事件以并行的方式派发到每个已经注册的事件处理器上进行处理。
        /// </summary>
        Parallel
    }
}
