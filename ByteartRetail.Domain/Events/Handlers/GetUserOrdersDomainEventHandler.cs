using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using ByteartRetail.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.Handlers
{
    [HandlesAsynchronously]
    public class GetUserOrdersDomainEventHandler : IDomainEventHandler<GetUserOrdersEvent>
    {
        private readonly ISalesOrderRepository salesOrderRepository;

        public GetUserOrdersDomainEventHandler(ISalesOrderRepository salesOrderRepository)
        {
            this.salesOrderRepository = salesOrderRepository;
        }

        public void Handle(GetUserOrdersEvent evnt)
        {
            var user = evnt.Source as User;
            evnt.SalesOrders = this.salesOrderRepository.FindSalesOrdersByUser(user);
        }
    }
}
