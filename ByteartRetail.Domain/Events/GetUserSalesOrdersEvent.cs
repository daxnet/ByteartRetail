using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events
{
    [Serializable]
    public class GetUserSalesOrdersEvent : DomainEvent
    {
        public GetUserSalesOrdersEvent(IEntity source) : base(source) { }

        public IEnumerable<SalesOrder> SalesOrders { get; set; }


    }
}
