using ByteartRetail.Domain.Model;
using System.Collections.Generic;

namespace ByteartRetail.Domain.Repositories
{
    /// <summary>
    /// 表示继承于该接口的类型是作用在“用户”聚合根上的仓储类型。
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        #region Methods
        /// <summary>
        /// 确定指定的用户名是否存在。
        /// </summary>
        /// <param name="userName">待确定的用户名。</param>
        /// <returns>如果用户名存在，则返回true，否则返回false。</returns>
        bool UserNameExists(string userName);
        /// <summary>
        /// 确定指定的电子邮件地址是否存在。
        /// </summary>
        /// <param name="userName">待确定的电子邮件地址。</param>
        /// <returns>如果电子邮件地址存在，则返回true，否则返回false。</returns>
        bool EmailExists(string email);
        /// <summary>
        /// 确定指定的用户名和密码是否一致。
        /// </summary>
        /// <param name="userName">待确定的用户名。</param>
        /// <param name="password">待确定的密码。</param>
        /// <returns>如果两者一致，则返回true，否则返回false。</returns>
        bool CheckPassword(string userName, string password);
        /// <summary>
        /// 根据指定的用户名，获取用户实体。
        /// </summary>
        /// <param name="userName">需要获取的用户的用户名。</param>
        /// <returns>用户实体。</returns>
        User GetUserByName(string userName);
        /// <summary>
        /// 根据指定的电子邮件地址，获取用户实体。
        /// </summary>
        /// <param name="email">需要获取的用户的电子邮件地址。</param>
        /// <returns>用户实体。</returns>
        User GetUserByEmail(string email);
        #endregion
    }
}
