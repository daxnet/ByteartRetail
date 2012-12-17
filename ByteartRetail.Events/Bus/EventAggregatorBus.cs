using ByteartRetail.Events.Handlers;
using ByteartRetail.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Bus
{
    public class EventAggregatorBus : IBus
    {
        #region Private Fields
        private static Dictionary<Type, IEventAggregator> eventAggregators = new Dictionary<Type, IEventAggregator>();
        #endregion

        public EventAggregatorBus()
        {
            
        }

        public static void RegisterAggregator<TEvent>()
            where TEvent : class, IEvent
        {
            if (eventAggregators.ContainsKey(typeof(TEvent)))
                return;
            IEventAggregator<TEvent> eventAggregator = ServiceLocator
                .Instance
                .GetService<IEventAggregator<TEvent>>(); // 从IoC容器中解析应用于给定事件类型的事件聚合器实例。

            eventAggregators.Add(typeof(TEvent), eventAggregator); // 将事件聚合器实例添加到线程的本地存储中。
        }

        #region IBus Members

        public void Publish<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            var eventAggregator = eventAggregators[typeof(TEvent)];
            if (eventAggregator != null)
                eventAggregator.DispatchEvent(evnt);
        }

        public void Publish<TEvent>(IEnumerable<TEvent> evnts) where TEvent : class, IEvent
        {
            foreach (var evnt in evnts)
                Publish(evnt);
        }

        public bool IsDistributedTransactionSupported
        {
            get { return false; }
        }

        #endregion
    }
}
