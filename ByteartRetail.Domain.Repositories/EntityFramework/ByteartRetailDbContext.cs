using ByteartRetail.Domain.Model;
using ByteartRetail.Domain.Repositories.EntityFramework.ModelConfigurations;
using System.Data.Entity;

namespace ByteartRetail.Domain.Repositories.EntityFramework
{
    /// <summary>
    /// 表示专用于Byteart Retail案例的数据访问上下文。
    /// </summary>
    public sealed class ByteartRetailDbContext : DbContext
    {
        #region Ctor
        /// <summary>
        /// 构造函数，初始化一个新的<c>ByteartRetailDbContext</c>实例。
        /// </summary>
        public ByteartRetailDbContext()
            : base("ByteartRetail")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a set of <c>User</c>s.
        /// </summary>
        public DbSet<User> Users
        {
            get { return Set<User>(); }
        }

        /// <summary>
        /// Gets a set of <c>SalesOrder</c>s.
        /// </summary>
        public DbSet<SalesOrder> SalesOrders
        {
            get { return Set<SalesOrder>(); }
        }

        /// <summary>
        /// Gets a set of <c>Laptop</c>s.
        /// </summary>
        public DbSet<Product> Products
        {
            get { return Set<Product>(); }
        }

        public DbSet<Category> Categories
        {
            get { return Set<Category>(); }
        }

        public DbSet<Categorization> Categorizations
        {
            get { return Set<Categorization>(); }
        }
        /// <summary>
        /// Gets a set of <c>ShoppingCart</c>s.
        /// </summary>
        public DbSet<ShoppingCart> ShoppingCarts
        {
            get { return Set<ShoppingCart>(); }
        }

        public DbSet<UserRole> UserRoles
        {
            get { return Set<UserRole>(); }
        }

        public DbSet<Role> Roles
        {
            get { return Set<Role>(); }
        }
        #endregion

        #region Protected Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Configurations
                .Add(new UserTypeConfiguration())
                .Add(new ProductTypeConfiguration())
                .Add(new CategoryTypeConfiguration())
                .Add(new CategorizationTypeConfiguration())
                .Add(new SalesLineTypeConfiguration())
                .Add(new SalesOrderTypeConfiguration())
                .Add(new ShoppingCartItemTypeConfiguration())
                .Add(new ShoppingCartTypeConfiguration())
                .Add(new RoleTypeConfiguration())
                .Add(new UserRoleTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
