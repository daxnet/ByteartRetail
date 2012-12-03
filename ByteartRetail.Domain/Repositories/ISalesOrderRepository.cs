using ByteartRetail.Domain.Model;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示用于“销售订单”聚合根的仓储接口。
    /// </summary>
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {
        #region Methods
        /// <summary>
        /// 查找属于指定用户的所有销售订单。
        /// </summary>
        /// <param name="user">用户。</param>
        /// <returns>指定用户的所有销售订单。</returns>
        IEnumerable<SalesOrder> FindSalesOrdersByUser(User user);
        /// <summary>
        /// 根据销售订单的ID值获取销售订单。
        /// </summary>
        /// <param name="orderID">销售订单的ID值。</param>
        /// <returns>销售订单。</returns>
        SalesOrder GetSalesOrderByID(Guid orderID);
        #endregion
    }
}
