using System;

namespace ByteartRetail.Infrastructure.Caching
{
    /// <summary>
    /// 表示由此特性所描述的方法，能够获得来自Byteart Retail基础结构层所提供的缓存功能。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class CachingAttribute : Attribute
    {
        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>CachingAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        public CachingAttribute(CachingMethod method)
        {
            this.Method = method;
        }
        /// <summary>
        /// 初始化一个新的<c>CachingAttribute</c>类型。
        /// </summary>
        /// <param name="method">缓存方式。</param>
        /// <param name="correspondingMethodNames">与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。</param>
        public CachingAttribute(CachingMethod method, params string[] correspondingMethodNames)
            : this(method)
        {
            this.CorrespondingMethodNames = correspondingMethodNames;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置缓存方式。
        /// </summary>
        public CachingMethod Method { get; set; }
        /// <summary>
        /// 获取或设置一个<see cref="Boolean"/>值，该值表示当缓存方式为Put时，是否强制将值写入缓存中。
        /// </summary>
        public bool Force { get; set; }
        /// <summary>
        /// 获取或设置与当前缓存方式相关的方法名称。注：此参数仅在缓存方式为Remove时起作用。
        /// </summary>
        public string[] CorrespondingMethodNames { get; set; }
        #endregion
    }
}
