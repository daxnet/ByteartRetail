using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Events
{
    [Serializable]
    public class OrderConfirmedEvent : Event
    {
        public string EmailAddress { get; set; }
        public Guid SalesOrderID { get; set; }
        public DateTime DateConfirmed { get; set; }

        public OrderConfirmedEvent() { }

        public OrderConfirmedEvent(string emailAddress, Guid salesOrderID, DateTime dateConfirmed)
        {
            this.EmailAddress = emailAddress;
            this.SalesOrderID = salesOrderID;
            this.DateConfirmed = dateConfirmed;
        }
    }
}
