using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;

namespace ByteartRetail.Infrastructure.InterceptionBehaviors
{
    /// <summary>
    /// 表示用于异常日志记录的拦截行为。
    /// </summary>
    public class ExceptionLoggingBehavior : IInterceptionBehavior
    {
        #region IInterceptionBehavior Members
        /// <summary>
        /// 获取当前行为需要拦截的对象类型接口。
        /// </summary>
        /// <returns>所有需要拦截的对象类型接口。</returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// 通过实现此方法来拦截调用并执行所需的拦截行为。
        /// </summary>
        /// <param name="input">调用拦截目标时的输入信息。</param>
        /// <param name="getNext">通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>从拦截目标获得的返回信息。</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var methodReturn = getNext().Invoke(input, getNext);
            if (methodReturn.Exception != null)
            {
                Utils.Log(methodReturn.Exception);
            }
            return methodReturn;
        }
        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前拦截行为被调用时，是否真的需要执行
        /// 某些操作。
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }

        #endregion
    }
}
