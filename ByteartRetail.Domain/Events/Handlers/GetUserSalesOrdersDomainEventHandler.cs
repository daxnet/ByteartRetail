using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.Handlers
{
    [HandlesAsynchronously]
    public class GetUserSalesOrdersDomainEventHandler : DomainEventHandler<GetUserSalesOrdersEvent>
    {
        private readonly ISalesOrderRepository salesOrderRepository;

        public GetUserSalesOrdersDomainEventHandler(ISalesOrderRepository salesOrderRepository)
        {
            this.salesOrderRepository = salesOrderRepository;
        }

        public override void Handle(GetUserSalesOrdersEvent evnt)
        {
            var user = evnt.Source as User;
            evnt.SalesOrders = this.salesOrderRepository.FindSalesOrdersByUser(user);
        }
    }
}
