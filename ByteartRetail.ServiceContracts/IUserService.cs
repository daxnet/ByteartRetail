using ByteartRetail.DataObjects;
using ByteartRetail.Infrastructure;
using ByteartRetail.Infrastructure.Caching;
using System;
using System.ServiceModel;

namespace ByteartRetail.ServiceContracts
{
    /// <summary>
    /// 表示与“用户”相关的应用层服务契约。
    /// </summary>
    [ServiceContract(Namespace="http://www.ByteartRetail.com")]
    public interface IUserService : IApplicationServiceContract
    {
        #region Methods
        /// <summary>
        /// 根据指定的用户信息，创建用户对象。
        /// </summary>
        /// <param name="userDataObject">包含了用户信息的数据传输对象。</param>
        /// <returns>已创建用户对象的全局唯一标识。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObjectList CreateUsers(UserDataObjectList userDataObjects);

        /// <summary>
        /// 校验指定的用户用户名与密码是否一致。
        /// </summary>
        /// <param name="userName">用户用户名。</param>
        /// <param name="password">用户密码。</param>
        /// <returns>如果校验成功，则返回true，否则返回false。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool ValidateUser(string userName, string password);

        /// <summary>
        /// 禁用指定用户。
        /// </summary>
        /// <param name="userDataObject">需要禁用的用户。</param>
        /// <returns>如果成功，则返回true，否则返回false。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool DisableUser(UserDataObject userDataObject);

        /// <summary>
        /// 启用指定用户。
        /// </summary>
        /// <param name="userDataObject">需要启用的用户。</param>
        /// <returns>如果成功，则返回true，否则返回false。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        bool EnableUser(UserDataObject userDataObject);

        /// <summary>
        /// 根据指定的用户信息，更新用户对象。
        /// </summary>
        /// <param name="userDataObjects">需要更新的用户对象。</param>
        /// <returns>已更新的用户对象。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObjectList UpdateUsers(UserDataObjectList userDataObjects);

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="userDataObjects">需要删除的用户对象。</param>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteUsers(UserDataObjectList userDataObjects);

        /// <summary>
        /// 根据用户的全局唯一标识获取用户信息。
        /// </summary>
        /// <param name="ID">用户的全局唯一标识。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObject GetUserByKey(Guid ID, QuerySpec spec);

        /// <summary>
        /// 根据用户的电子邮件地址获取用户信息。
        /// </summary>
        /// <param name="email">用户的电子邮件地址。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObject GetUserByEmail(string email, QuerySpec spec);

        /// <summary>
        /// 根据用户的用户名获取用户信息。
        /// </summary>
        /// <param name="userName">用户的用户名。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObject GetUserByName(string userName, QuerySpec spec);

        /// <summary>
        /// 获取所有用户的信息。
        /// </summary>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了所有用户信息的数据传输对象列表。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        UserDataObjectList GetUsers(QuerySpec spec);

        /// <summary>
        /// 获取所有角色。
        /// </summary>
        /// <returns>所有角色。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        RoleDataObjectList GetRoles();

        /// <summary>
        /// 根据指定的ID值，获取角色。
        /// </summary>
        /// <param name="id">指定的角色ID值。</param>
        /// <returns>角色。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        RoleDataObject GetRoleByKey(Guid id);

        /// <summary>
        /// 创建角色。
        /// </summary>
        /// <param name="roleDataObjects">需要创建的角色。</param>
        /// <returns>已创建的角色。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        RoleDataObjectList CreateRoles(RoleDataObjectList roleDataObjects);

        /// <summary>
        /// 更新角色。
        /// </summary>
        /// <param name="roleDataObjects">需要更新的角色。</param>
        /// <returns>已更新的角色。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        RoleDataObjectList UpdateRoles(RoleDataObjectList roleDataObjects);

        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="roleIDs">需要删除的角色ID值列表。</param>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        void DeleteRoles(IDList roleIDs);

        /// <summary>
        /// 将指定的用户赋予指定的角色。
        /// </summary>
        /// <param name="userID">需要赋予角色的用户ID值。</param>
        /// <param name="roleID">需要向用户赋予的角色ID值。</param>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Caching(CachingMethod.Remove, "GetUserRoleByUserName")]
        void AssignRole(Guid userID, Guid roleID);

        /// <summary>
        /// 将指定的用户从角色中移除。
        /// </summary>
        /// <param name="userID">用户ID值。</param>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Caching(CachingMethod.Remove, "GetUserRoleByUserName")]
        void UnassignRole(Guid userID);

        /// <summary>
        /// 根据指定的用户名，获取该用户所属的角色。
        /// </summary>
        /// <param name="userName">用户名。</param>
        /// <returns>角色。</returns>
        [OperationContract]
        [FaultContract(typeof(FaultData))]
        [Caching(CachingMethod.Get)]
        RoleDataObject GetUserRoleByUserName(string userName);

        #endregion
    }
}
