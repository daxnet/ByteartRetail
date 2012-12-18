using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“用户角色关系”领域概念的聚合根。
    /// </summary>
    public class UserRole : AggregateRoot
    {
        #region Private Fields
        private Guid userID;
        private Guid roleID;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>UserRole</c>实例。
        /// </summary>
        public UserRole() { }

        /// <summary>
        /// 初始化一个新的<c>UserRole</c>实例。
        /// </summary>
        /// <param name="userID">用户账户的ID。</param>
        /// <param name="roleID">角色的ID。</param>
        public UserRole(Guid userID, Guid roleID)
        {
            this.userID = userID;
            this.roleID = roleID;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置用户账户的ID值。
        /// </summary>
        public Guid UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 获取或设置角色的ID值。
        /// </summary>
        public Guid RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }
        #endregion
    }
}
