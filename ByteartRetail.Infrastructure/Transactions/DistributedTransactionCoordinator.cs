using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ByteartRetail.Infrastructure.Transactions
{
    internal sealed class DistributedTransactionCoordinator : TransactionCoordinator
    {
        private readonly TransactionScope scope = new TransactionScope();

        public DistributedTransactionCoordinator(params IUnitOfWork[] unitOfWorks)
            : base(unitOfWorks)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                scope.Dispose();
        }


        public override void Commit()
        {
            base.Commit();
            scope.Complete();
        }

    }
}
