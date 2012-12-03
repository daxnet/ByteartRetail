using System;

namespace ByteartRetail.Domain.Model
{
    /// <summary>
    /// 表示“角色”领域概念的聚合根。
    /// </summary>
    public class Role : AggregateRoot
    {
        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>Role</c>实例。
        /// </summary>
        public Role() { }
        /// <summary>
        /// 初始化一个新的<c>Role</c>实例。
        /// </summary>
        /// <param name="name">角色名称。</param>
        /// <param name="description">角色描述。</param>
        public Role(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        #endregion

        #region Public Properties
        private string name;
        /// <summary>
        /// 获取或设置角色名称。
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        private string description;
        /// <summary>
        /// 获取或设置角色描述。
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        #endregion
    }
}
