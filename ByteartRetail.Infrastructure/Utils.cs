using System;

namespace ByteartRetail.Infrastructure
{
    /// <summary>
    /// 表示用于整个Byteart Retail系统的工具类。
    /// </summary>
    public static class Utils
    {
        #region Private Fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ByteartRetail.Logger");
        #endregion

        #region Public Static Methods
        /// <summary>
        /// 将指定的字符串信息写入日志。
        /// </summary>
        /// <param name="message">需要写入日志的字符串信息。</param>
        public static void Log(string message)
        {
            log.Info(message);
        }
        /// <summary>
        /// 将指定的<see cref="Exception"/>实例详细信息写入日志。
        /// </summary>
        /// <param name="ex">需要将详细信息写入日志的<see cref="Exception"/>实例。</param>
        public static void Log(Exception ex)
        {
            log.Error("Exception caught", ex);
        }
        #endregion
    }
}
