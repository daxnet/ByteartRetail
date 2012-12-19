using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events.Handlers
{
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        #region IEventHandler<TEvent> Members

        public abstract void Handle(TEvent evnt);

        #endregion

        #region IEventHandler Members

        public void Handle(IEvent evnt)
        {
            if (evnt is TEvent)
                Handle(evnt as TEvent);
        }

        #endregion
    }
}
