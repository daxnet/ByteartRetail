using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ByteartRetail.Domain.Events.Handlers
{
    public class OrderDispatchedDomainEventHandler : DomainEventHandler<OrderDispatchedEvent>
    {
        private readonly ISalesOrderRepository salesOrderRepository;

        public OrderDispatchedDomainEventHandler(ISalesOrderRepository salesOrderRepository)
        {
            this.salesOrderRepository = salesOrderRepository;
        }

        #region IEventHandler<OrderDispatchedEvent> Members

        public override void Handle(OrderDispatchedEvent evnt)
        {
            // TODO: 在此处可以执行一些与仓储相关的操作，然后根据操作结果更新evnt.Source
            // 中的相关属性。
        }

        #endregion
    }
}
