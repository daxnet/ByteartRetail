using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Infrastructure.Transactions
{
    internal sealed class SuppressedTransactionCoordinator : DisposableObject, ITransactionCoordinator
    {
        protected override void Dispose(bool disposing)
        {
            
        }

        #region IUnitOfWork Members

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return true;  }
        }

        public void Commit()
        {
            // do nothing
        }

        public void Rollback()
        {
            // do nothing
        }

        #endregion
    }
}
