using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events
{
    [Serializable]
    public class OrderDispatchedEvent : Event
    {
        public string EmailAddress { get; set; }
        public Guid SalesOrderID { get; set; }
        public DateTime DateDispatched { get; set; }

        public OrderDispatchedEvent() { }

        public OrderDispatchedEvent(string emailAddress, Guid salesOrderID, DateTime dateDispatched)
        {
            this.EmailAddress = emailAddress;
            this.SalesOrderID = salesOrderID;
            this.DateDispatched = dateDispatched;
        }
    }
}
