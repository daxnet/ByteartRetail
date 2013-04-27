using AutoMapper;
using ByteartRetail.DataObjects;
using ByteartRetail.Domain.Events;
using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using ByteartRetail.Domain.Services;
using ByteartRetail.Domain.Specifications;
using ByteartRetail.ServiceContracts;
using System;
using System.Linq;

namespace ByteartRetail.Application.Implementation
{
    /// <summary>
    /// 表示与“客户”相关的应用层服务的一种实现。
    /// </summary>
    public class UserServiceImpl : ApplicationService, IUserService
    {
        #region Private Fields
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly ISalesOrderRepository salesOrderRepository;
        private readonly IDomainService domainService;
        private readonly IDomainEventHandler<GetUserSalesOrdersEvent>[] getUserSalesOrdersEventHandlers;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>UserServiceImpl</c>实例。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userRepository"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="salesOrderRepository"></param>
        /// <param name="domainService"></param>
        public UserServiceImpl(IRepositoryContext context,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IShoppingCartRepository shoppingCartRepository,
            ISalesOrderRepository salesOrderRepository,
            IDomainService domainService,
            IDomainEventHandler<GetUserSalesOrdersEvent>[] getUserSalesOrdersEventHandlers)
            : base(context)
        {
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
            this.roleRepository = roleRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.salesOrderRepository = salesOrderRepository;
            this.domainService = domainService;
            this.getUserSalesOrdersEventHandlers = getUserSalesOrdersEventHandlers;
            DomainEvent.Subscribe<GetUserSalesOrdersEvent>(getUserSalesOrdersEventHandlers);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DomainEvent.UnsubscribeAll<GetUserSalesOrdersEvent>();
            }
        }

        #region IUserService Members
        /// <summary>
        /// 根据指定的客户信息，创建客户对象。
        /// </summary>
        /// <param name="userDataObjects">包含了客户信息的数据传输对象。</param>
        /// <returns>已创建客户对象的全局唯一标识。</returns>
        public UserDataObjectList CreateUsers(UserDataObjectList userDataObjects)
        {
            if (userDataObjects == null)
                throw new ArgumentNullException("userDataObjects");
            return PerformCreateObjects<UserDataObjectList, UserDataObject, User>(userDataObjects, 
                userRepository, 
                dto =>
                {
                    if (dto.DateRegistered == null)
                        dto.DateRegistered = DateTime.Now;
                },
                ar =>
                {
                    var shoppingCart = ar.CreateShoppingCart();
                    shoppingCartRepository.Add(shoppingCart);
                });
        }

        /// <summary>
        /// 校验指定的客户用户名与密码是否一致。
        /// </summary>
        /// <param name="userName">客户用户名。</param>
        /// <param name="password">客户密码。</param>
        /// <returns>如果校验成功，则返回true，否则返回false。</returns>
        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException("userName");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return userRepository.CheckPassword(userName, password);
        }

        /// <summary>
        /// 禁用指定用户。
        /// </summary>
        /// <param name="userDataObject">需要禁用的用户。</param>
        /// <returns>如果成功，则返回true，否则返回false。</returns>
        public bool DisableUser(UserDataObject userDataObject)
        {
            if (userDataObject == null)
                throw new ArgumentNullException("userDataObject");
            User user = null;
            if (!IsEmptyGuidString(userDataObject.ID))
                user = userRepository.GetByKey(new Guid(userDataObject.ID));
            else if (!string.IsNullOrEmpty(userDataObject.UserName))
                user = userRepository.GetUserByName(userDataObject.UserName);
            else if (!string.IsNullOrEmpty(userDataObject.Email))
                user = userRepository.GetUserByEmail(userDataObject.Email);
            else
                throw new ArgumentNullException("userDataObject", "Either ID, UserName or Email should be specified.");
            user.Disable();
            userRepository.Update(user);
            Context.Commit();
            return user.IsDisabled;
        }

        /// <summary>
        /// 启用指定用户。
        /// </summary>
        /// <param name="userDataObject">需要启用的用户。</param>
        /// <returns>如果成功，则返回true，否则返回false。</returns>
        public bool EnableUser(UserDataObject userDataObject)
        {
            if (userDataObject == null)
                throw new ArgumentNullException("userDataObject");
            User user = null;
            if (!IsEmptyGuidString(userDataObject.ID))
                user = userRepository.GetByKey(new Guid(userDataObject.ID));
            else if (!string.IsNullOrEmpty(userDataObject.UserName))
                user = userRepository.GetUserByName(userDataObject.UserName);
            else if (!string.IsNullOrEmpty(userDataObject.Email))
                user = userRepository.GetUserByEmail(userDataObject.Email);
            else
                throw new ArgumentNullException("userDataObject", "Either ID, UserName or Email should be specified.");
            user.Enable();
            userRepository.Update(user);
            Context.Commit();
            return user.IsDisabled;
        }

        /// <summary>
        /// 根据指定的用户信息，更新用户对象。
        /// </summary>
        /// <param name="userDataObjects">需要更新的用户对象。</param>
        /// <returns>已更新的用户对象。</returns>
        public UserDataObjectList UpdateUsers(UserDataObjectList userDataObjects)
        {
            return PerformUpdateObjects<UserDataObjectList, UserDataObject, User>(userDataObjects, userRepository,
                udo => udo.ID,
                (u, udo) =>
                {
                    if (!string.IsNullOrEmpty(udo.Contact))
                        u.Contact = udo.Contact;
                    if (!string.IsNullOrEmpty(udo.PhoneNumber))
                        u.PhoneNumber = udo.PhoneNumber;
                    if (udo.ContactAddress != null)
                    {
                        if (!string.IsNullOrEmpty(udo.ContactAddress.City))
                            u.ContactAddress.City = udo.ContactAddress.City;
                        if (!string.IsNullOrEmpty(udo.ContactAddress.Country))
                            u.ContactAddress.Country = udo.ContactAddress.Country;
                        if (!string.IsNullOrEmpty(udo.ContactAddress.State))
                            u.ContactAddress.State = udo.ContactAddress.State;
                        if (!string.IsNullOrEmpty(udo.ContactAddress.Street))
                            u.ContactAddress.Street = udo.ContactAddress.Street;
                        if (!string.IsNullOrEmpty(udo.ContactAddress.Zip))
                            u.ContactAddress.Zip = udo.ContactAddress.Zip;
                    }
                    if (udo.DeliveryAddress != null)
                    {
                        if (!string.IsNullOrEmpty(udo.DeliveryAddress.City))
                            u.DeliveryAddress.City = udo.DeliveryAddress.City;
                        if (!string.IsNullOrEmpty(udo.DeliveryAddress.Country))
                            u.DeliveryAddress.Country = udo.DeliveryAddress.Country;
                        if (!string.IsNullOrEmpty(udo.DeliveryAddress.State))
                            u.DeliveryAddress.State = udo.DeliveryAddress.State;
                        if (!string.IsNullOrEmpty(udo.DeliveryAddress.Street))
                            u.DeliveryAddress.Street = udo.DeliveryAddress.Street;
                        if (!string.IsNullOrEmpty(udo.DeliveryAddress.Zip))
                            u.DeliveryAddress.Zip = udo.DeliveryAddress.Zip;
                    }
                    if (udo.DateLastLogon != null)
                        u.DateLastLogon = udo.DateLastLogon;
                    if (udo.DateRegistered != null)
                        u.DateRegistered = udo.DateRegistered.Value;
                    if (!string.IsNullOrEmpty(udo.Email))
                        u.Email = udo.Email;
                    if (udo.IsDisabled != null)
                    {
                        if (udo.IsDisabled.Value)
                            u.Disable();
                        else
                            u.Enable();
                    }
                    if (!string.IsNullOrEmpty(udo.Password))
                        u.Password = udo.Password;
                });
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="userDataObjects">需要删除的用户对象。</param>
        public void DeleteUsers(UserDataObjectList userDataObjects)
        {
            if (userDataObjects == null)
                throw new ArgumentNullException("userDataObjects");
            foreach (var userDataObject in userDataObjects)
            {
                User user = null;
                if (!IsEmptyGuidString(userDataObject.ID))
                    user = userRepository.GetByKey(new Guid(userDataObject.ID));
                else if (!string.IsNullOrEmpty(userDataObject.UserName))
                    user = userRepository.GetUserByName(userDataObject.UserName);
                else if (!string.IsNullOrEmpty(userDataObject.Email))
                    user = userRepository.GetUserByEmail(userDataObject.Email);
                else
                    throw new ArgumentNullException("userDataObject", "Either ID, UserName or Email should be specified.");
                var userRole = userRoleRepository.Find(Specification<UserRole>.Eval(ur => ur.UserID == user.ID));
                if (userRole != null)
                    userRoleRepository.Remove(userRole);
                userRepository.Remove(user);
            }
            Context.Commit();
        }

        /// <summary>
        /// 根据用户的全局唯一标识获取用户信息。
        /// </summary>
        /// <param name="ID">用户的全局唯一标识。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        public UserDataObject GetUserByKey(Guid ID, QuerySpec spec)
        {
            var user = userRepository.GetByKey(ID);
            var userDataObject = Mapper.Map<User, UserDataObject>(user);
            if (spec.IsVerbose())
            {
                var role = userRoleRepository.GetRoleForUser(user);
                if (role != null)
                {
                    userDataObject.Role = Mapper.Map<Role, RoleDataObject>(role);
                }
            }
            return userDataObject;
        }

        /// <summary>
        /// 根据用户的电子邮件地址获取用户信息。
        /// </summary>
        /// <param name="email">用户的电子邮件地址。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        public UserDataObject GetUserByEmail(string email, QuerySpec spec)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("email");
            var user = userRepository.GetUserByEmail(email);
            var userDataObject = Mapper.Map<User, UserDataObject>(user);
            if (spec.IsVerbose())
            {
                var role = userRoleRepository.GetRoleForUser(user);
                if (role != null)
                {
                    userDataObject.Role = Mapper.Map<Role, RoleDataObject>(role);
                }
            }
            return userDataObject;
        }

        /// <summary>
        /// 根据用户的用户名获取用户信息。
        /// </summary>
        /// <param name="userName">用户的用户名。</param>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了用户信息的数据传输对象。</returns>
        public UserDataObject GetUserByName(string userName, QuerySpec spec)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException("userName");
            var user = userRepository.GetUserByName(userName);
            var userDataObject = Mapper.Map<User, UserDataObject>(user);
            if (spec.IsVerbose())
            {
                var role = userRoleRepository.GetRoleForUser(user);
                if (role != null)
                {
                    userDataObject.Role = Mapper.Map<Role, RoleDataObject>(role);
                }
            }
            return userDataObject;
        }

        /// <summary>
        /// 获取所有用户的信息。
        /// </summary>
        /// <param name="spec">查询方式。</param>
        /// <returns>包含了所有用户信息的数据传输对象列表。</returns>
        public UserDataObjectList GetUsers(QuerySpec spec)
        {
            var users = userRepository.FindAll();
            if (users == null)
                return null;
            var ret = new UserDataObjectList();
            foreach (var user in users)
            {
                var userDataObject = Mapper.Map<User, UserDataObject>(user);
                if (spec.IsVerbose())
                {
                    var role = userRoleRepository.GetRoleForUser(user);
                    if (role != null)
                    {
                        userDataObject.Role = Mapper.Map<Role, RoleDataObject>(role);
                    }
                }
                ret.Add(userDataObject);
            }
            return ret;
        }

        /// <summary>
        /// 根据指定的ID值，获取角色。
        /// </summary>
        /// <param name="id">指定的角色ID值。</param>
        /// <returns>角色。</returns>
        public RoleDataObject GetRoleByKey(Guid id)
        {
            return Mapper.Map<Role, RoleDataObject>(roleRepository.GetByKey(id));
        }

        /// <summary>
        /// 获取所有角色。
        /// </summary>
        /// <returns>所有角色。</returns>
        public RoleDataObjectList GetRoles()
        {
            var roles = roleRepository.FindAll();
            RoleDataObjectList result = null;
            if (roles != null &&
                roles.Count() > 0)
            {
                result = new RoleDataObjectList();
                roles.ToList()
                    .ForEach(r => result.Add(Mapper.Map<Role, RoleDataObject>(r)));
            }
            return result;
        }

        /// <summary>
        /// 创建角色。
        /// </summary>
        /// <param name="roleDataObjects">需要创建的角色。</param>
        /// <returns>已创建的角色。</returns>
        public RoleDataObjectList CreateRoles(RoleDataObjectList roleDataObjects)
        {
            return PerformCreateObjects<RoleDataObjectList, RoleDataObject, Role>(roleDataObjects, roleRepository);
        }

        /// <summary>
        /// 更新角色。
        /// </summary>
        /// <param name="roleDataObjects">需要更新的角色。</param>
        /// <returns>已更新的角色。</returns>
        public RoleDataObjectList UpdateRoles(RoleDataObjectList roleDataObjects)
        {
            return PerformUpdateObjects<RoleDataObjectList, RoleDataObject, Role>(roleDataObjects,
                roleRepository,
                rdo => rdo.ID,
                (r, rdo) =>
                {
                    if (!string.IsNullOrEmpty(rdo.Name))
                        r.Name = rdo.Name;
                    if (!string.IsNullOrEmpty(rdo.Description))
                        r.Description = rdo.Description;
                });
        }

        /// <summary>
        /// 删除角色。
        /// </summary>
        /// <param name="roleIDs">需要删除的角色ID值列表。</param>
        public void DeleteRoles(IDList roleIDs)
        {
            PerformDeleteObjects<Role>(roleIDs,
                roleRepository,
                id =>
                {
                    var userRole = userRoleRepository.Find(Specification<UserRole>.Eval(ur => ur.RoleID == id));
                    if (userRole != null)
                        userRoleRepository.Remove(userRole);
                });
        }

        /// <summary>
        /// 将指定的用户赋予指定的角色。
        /// </summary>
        /// <param name="userID">需要赋予角色的用户ID值。</param>
        /// <param name="roleID">需要向用户赋予的角色ID值。</param>
        public void AssignRole(Guid userID, Guid roleID)
        {
            var user = userRepository.GetByKey(userID);
            var role = roleRepository.GetByKey(roleID);
            domainService.AssignRole(user, role);
        }

        /// <summary>
        /// 将指定的用户从角色中移除。
        /// </summary>
        /// <param name="userID">用户ID值。</param>
        public void UnassignRole(Guid userID)
        {
            var user = userRepository.GetByKey(userID);
            domainService.UnassignRole(user);
        }

        /// <summary>
        /// 根据指定的用户名，获取该用户所属的角色。
        /// </summary>
        /// <param name="userName">用户名。</param>
        /// <returns>角色。</returns>
        public RoleDataObject GetUserRoleByUserName(string userName)
        {
            User user = userRepository.GetUserByName(userName);
            var role = userRoleRepository.GetRoleForUser(user);
            return Mapper.Map<Role, RoleDataObject>(role);
        }

        public SalesOrderDataObjectList GetSalesOrders(string userName)
        {
            User user = userRepository.GetUserByName(userName);
            var salesOrders = user.SalesOrders;
            SalesOrderDataObjectList result = new SalesOrderDataObjectList();
            foreach (var so in salesOrders)
            {
                result.Add(Mapper.Map<SalesOrder, SalesOrderDataObject>(so));
            }
            return result;
        }

        #endregion
    }
}
