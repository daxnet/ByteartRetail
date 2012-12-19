using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    public class OrderConfirmedEvent : DomainEvent
    {
        public OrderConfirmedEvent(IEntity source) : base(source) { }

        public DateTime ConfirmedDate { get; set; }
    }
}
