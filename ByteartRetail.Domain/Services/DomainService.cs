using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories;
using ByteartRetail.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ByteartRetail.Domain.Services
{
    /// <summary>
    /// 表示用于Byteart Retail领域模型中的领域服务类型。
    /// </summary>
    public class DomainService : IDomainService
    {
        #region Private Fields
        private readonly IRepositoryContext repositoryContext;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategorizationRepository categorizationRepository;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IShoppingCartItemRepository shoppingCartItemRepository;
        private readonly ISalesOrderRepository salesOrderRepository;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>DomainService</c>类型的实例。
        /// </summary>
        /// <param name="repositoryContext">仓储上下文。</param>
        /// <param name="productRepository">商品仓储。</param>
        /// <param name="categoryRepository">商品分类仓储。</param>
        /// <param name="categorizationRepository">商品分类关系仓储。</param>
        /// <param name="userRepository">用户仓储。</param>
        /// <param name="roleRepository">角色仓储。</param>
        /// <param name="userRoleRepository">用户角色关系仓储。</param>
        /// <param name="shoppingCartItemRepository">购物篮项目仓储。</param>
        /// <param name="salesOrderRepository">销售订单仓储。</param>
        public DomainService(IRepositoryContext repositoryContext,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ICategorizationRepository categorizationRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IShoppingCartItemRepository shoppingCartItemRepository,
            ISalesOrderRepository salesOrderRepository)
        {
            this.repositoryContext = repositoryContext;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.categorizationRepository = categorizationRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRepository = userRoleRepository;
            this.shoppingCartItemRepository = shoppingCartItemRepository;
            this.salesOrderRepository = salesOrderRepository;
        }
        #endregion

        #region IDomainService Members
        /// <summary>
        /// 将指定的商品归类到指定的商品分类中。
        /// </summary>
        /// <param name="product">需要归类的商品。</param>
        /// <param name="category">商品分类。</param>
        /// <returns>用以表述商品及其分类之间关系的实体。</returns>
        public Categorization Categorize(Product product, Category category)
        {
            if (product == null)
                throw new ArgumentNullException("product");
            if (category == null)
                throw new ArgumentNullException("category");
            var categorization = categorizationRepository.Find(Specification<Categorization>.Eval(c => c.ProductID == product.ID));
            if (categorization == null)
            {
                categorization = Categorization.CreateCategorization(product, category);
                categorizationRepository.Add(categorization);
            }
            else
            {
                categorization.CategoryID = category.ID;
                categorizationRepository.Update(categorization);
            }
            repositoryContext.Commit();
            return categorization;
        }
        /// <summary>
        /// 将指定的商品从其所属的商品分类中移除。
        /// </summary>
        /// <param name="product">商品。</param>
        /// <param name="category">分类，若为NULL，则表示从所有分类中移除。</param>
        public void Uncategorize(Product product, Category category = null)
        {
            Expression<Func<Categorization, bool>> specExpression = null;
            if (category == null)
                specExpression = p => p.ProductID == product.ID;
            else
                specExpression = p => p.ProductID == product.ID && p.CategoryID == category.ID;
            var categorization = categorizationRepository.Find(Specification<Categorization>.Eval(specExpression));
            if (categorization != null)
                categorizationRepository.Remove(categorization);
            repositoryContext.Commit();
        }
        /// <summary>
        /// 通过指定的用户及其所拥有的购物篮实体，创建销售订单。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="shoppingCart">购物篮实体。</param>
        /// <returns>销售订单实体。</returns>
        public SalesOrder CreateSalesOrder(User user, ShoppingCart shoppingCart)
        {
            var shoppingCartItems = shoppingCartItemRepository.FindItemsByCart(shoppingCart);
            if (shoppingCartItems == null ||
                shoppingCartItems.Count() == 0)
                throw new InvalidOperationException("购物篮中没有任何物品。");

            var salesOrder = new SalesOrder();
            salesOrder.SalesLines = new List<SalesLine>();

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var salesLine = shoppingCartItem.ConvertToSalesLine();
                salesLine.SalesOrder = salesOrder;
                salesOrder.SalesLines.Add(salesLine);
                shoppingCartItemRepository.Remove(shoppingCartItem);
            }
            salesOrder.User = user;
            salesOrder.Status = SalesOrderStatus.Paid;
            salesOrderRepository.Add(salesOrder);
            repositoryContext.Commit();
            return salesOrder;
        }
        /// <summary>
        /// 将指定的用户赋予特定的角色。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="role">角色实体。</param>
        /// <returns>用以表述用户及其角色之间关系的实体。</returns>
        public UserRole AssignRole(User user, Role role)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (role == null)
                throw new ArgumentNullException("role");
            UserRole userRole = userRoleRepository.Find(Specification<UserRole>.Eval(ur => ur.UserID == user.ID));
            if (userRole == null)
            {
                userRole = new UserRole(user.ID, role.ID);
                userRoleRepository.Add(userRole);
            }
            else
            {
                userRole.RoleID = role.ID;
                userRoleRepository.Update(userRole);
            }
            repositoryContext.Commit();
            return userRole;
        }
        /// <summary>
        /// 将指定的用户从角色中移除。
        /// </summary>
        /// <param name="user">用户实体。</param>
        /// <param name="role">角色实体，若为NULL，则表示从所有角色中移除。</param>
        public void UnassignRole(User user, Role role = null)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            Expression<Func<UserRole, bool>> specExpression = null;
            if (role == null)
                specExpression = ur => ur.UserID == user.ID;
            else
                specExpression = ur => ur.UserID == user.ID && ur.RoleID == role.ID;

            UserRole userRole = userRoleRepository.Find(Specification<UserRole>.Eval(specExpression));

            if (userRole != null)
            {
                userRoleRepository.Remove(userRole);
                repositoryContext.Commit();
            }
        }

        #endregion
    }
}
