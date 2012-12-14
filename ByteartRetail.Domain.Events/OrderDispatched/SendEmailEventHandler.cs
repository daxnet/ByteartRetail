using ByteartRetail.Infrastructure;
using System;

namespace ByteartRetail.Domain.Events.OrderDispatched
{
    /// <summary>
    /// 表示发送邮件的事件处理器。
    /// </summary>
    public class SendEmailEventHandler : IEventHandler<OrderDispatchedEvent>
    {
        #region IEventHandler<OrderDispatchedEvent> Members
        /// <summary>
        /// 处理给定的领域事件。
        /// </summary>
        /// <param name="event">需要处理的领域事件。</param>
        public void Handle(OrderDispatchedEvent @event)
        {
            try
            {
                // 此处仅为演示，所以邮件内容很简单。可以根据自己的实际情况做一些复杂的邮件功能，比如
                // 使用邮件模板或者邮件风格等。
                Utils.SendEmail(@event.DispatchedOrder.User.Email,
                    "Your Order Has Been Dispatched.",
                    string.Format("Your Order {0} has been dispatched on {1}. For more information please contact system administrator. Thank you for your order.",
                    @event.DispatchedOrder.ID.ToString().ToUpper(), @event.DispatchedDate));
            }
            catch(Exception ex)
            {
                // 如遇异常，直接记Log
                Utils.Log(ex);
            }
        }

        #endregion
    }
}
