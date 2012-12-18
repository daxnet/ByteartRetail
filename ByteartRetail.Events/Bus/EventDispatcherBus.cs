using ByteartRetail.Events.Handlers;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    public class EventDispatcherBus : IBus
    {
        #region Private Fields
        //private static Dictionary<Type, IEventDispatcher> eventDispatchers = new Dictionary<Type, IEventDispatcher>();
        private readonly List<IEventDispatcher> eventDispatchers = new List<IEventDispatcher>();
        #endregion

        public EventDispatcherBus(IEventDispatcher[] eventDispatchers)
        {
            this.eventDispatchers.AddRange(eventDispatchers);
        }

        //public static void RegisterDispatchersFor<TEvent>()
        //    where TEvent : class, IEvent
        //{
        //    if (eventDispatchers.ContainsKey(typeof(TEvent)))
        //        return;
        //    IEventDispatcher<TEvent> eventDispatcher = ServiceLocator
        //        .Instance
        //        .GetService<IEventDispatcher<TEvent>>(); // 从IoC容器中解析应用于给定事件类型的事件聚合器实例。

        //    eventDispatchers.Add(typeof(TEvent), eventDispatcher); // 将事件聚合器实例添加到线程的本地存储中。
        //}

        #region IBus Members

        public void Publish<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            var eventDispatcher = this.eventDispatchers.Where(p => p.EventType == evnt.GetType()).FirstOrDefault();
            if (eventDispatcher != null)
                eventDispatcher.DispatchEvent(evnt);
        }

        public void Publish<TEvent>(IEnumerable<TEvent> evnts) where TEvent : class, IEvent
        {
            foreach (var evnt in evnts)
                Publish<TEvent>(evnt);
        }

        public bool IsDistributedTransactionSupported
        {
            get { return false; }
        }

        #endregion
    }
}
