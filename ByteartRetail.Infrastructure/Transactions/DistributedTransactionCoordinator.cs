using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ByteartRetail.Infrastructure.Transactions
{
    internal sealed class DistributedTransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        private readonly TransactionScope scope = new TransactionScope();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                scope.Dispose();
        }

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public bool Committed
        {
            get { return true; }
        }

        public void Commit()
        {
            scope.Complete();
        }

        public void Rollback()
        {
            
        }

        #endregion
    }
}
