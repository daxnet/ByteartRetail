using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteartRetail.Infrastructure.Config
{
    /// <summary>
    /// 表示对Byteart Retail配置信息进行读取的单例类型。
    /// </summary>
    public sealed class ByteartRetailConfigurationReader
    {
        #region Private Fields
        private readonly ByteartRetailConfigSection configuration;
        private static readonly ByteartRetailConfigurationReader instance = new ByteartRetailConfigurationReader();
        #endregion

        #region Ctor
        static ByteartRetailConfigurationReader() { }

        private ByteartRetailConfigurationReader()
        {
            this.configuration = ByteartRetailConfigSection.Instance;
            if (this.configuration == null)
                throw new ConfigurationErrorsException("当前应用程序的配置文件中不存在与ByteartRetail相关的配置信息。");
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取在应用程序配置文件中所设置的每页允许显示的最大产品个数。
        /// </summary>
        public int ProductsPerPage
        {
            get { return configuration.Presentation.ProductsPageSize; }
        }

        public string EmailHost
        {
            get { return configuration.EmailClient.Host; }
        }

        public int EmailPort
        {
            get { return configuration.EmailClient.Port; }
        }

        public string EmailUserName
        {
            get { return configuration.EmailClient.UserName; }
        }

        public string EmailPassword
        {
            get { return configuration.EmailClient.Password; }
        }

        public string EmailSender
        {
            get { return configuration.EmailClient.Sender; }
        }

        public bool EmailEnableSsl
        {
            get { return configuration.EmailClient.EnableSsl; }
        }
        /// <summary>
        /// 获取<c>ByteartRetailConfigurationReader</c>的单例类型。
        /// </summary>
        public static ByteartRetailConfigurationReader Instance
        {
            get { return instance; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示指定的角色名称是否已在配置信息中注册。
        /// </summary>
        /// <param name="roleName">所需判断的角色名称。</param>
        /// <returns>如果指定的角色名称已经存在于配置信息中，则返回true，否则返回false。</returns>
        public bool RoleNameRegistered(string roleName)
        {
            foreach (PermissionKeyElement pke in this.configuration.PermissionKeys)
                if (pke.RoleName == roleName)
                    return true;
            return false;
        }
        /// <summary>
        /// 根据指定的角色名称获得所对应的权限键（Permission Key）名。
        /// </summary>
        /// <param name="roleName">角色名称。</param>
        /// <returns>权限键（Permission Key）名。</returns>
        public string GetKeyNameByRoleName(string roleName)
        {
            foreach (PermissionKeyElement pke in this.configuration.PermissionKeys)
                if (pke.RoleName == roleName)
                    return pke.KeyName;
            return null;
        }

        #endregion
    }
}
