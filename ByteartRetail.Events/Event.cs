using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events
{
    [Serializable]
    public abstract class Event : IEvent
    {
        private readonly Guid id = Guid.NewGuid();
        private readonly DateTime timeStamp = DateTime.UtcNow;

        public Event() { }

        #region IEvent Members

        public Guid ID
        {
            get { return id; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        #endregion
    }
}
