using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ByteartRetail.Web.Controllers
{
    /// <summary>
    /// 表示“控制器”Controller类型的基类型，所有Byteart Retail项目下的Controller都
    /// 应该继承于此基类型。
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        #region Private Constants
        private const string SUCCESS_PAGE_ACTION = "SuccessPage";
        private const string SUCCESS_PAGE_CONTROLLER = "Home";
        #endregion

        #region Protected Properties
        /// <summary>
        /// 获取当前登录用户的ID值。
        /// </summary>
        protected Guid UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return (Guid)Session["UserID"];
                else
                {
                    var id = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    Session["UserID"] = id;
                    return id;
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 将页面重定向到成功页面。
        /// </summary>
        /// <param name="pageTitle">需要在成功页面显示的成功信息。</param>
        /// <param name="action">成功信息显示后返回的Action名称。默认值：Index。</param>
        /// <param name="controller">成功信息显示后返回的Controller名称。默认值：Home。</param>
        /// <param name="waitSeconds">在成功页面停留的时间（秒）。默认值：3。</param>
        /// <returns>执行的Action Result。</returns>
        protected ActionResult RedirectToSuccess(string pageTitle, string action = "Index", string controller = "Home", int waitSeconds = 3)
        {
            return this.RedirectToAction(SUCCESS_PAGE_ACTION, SUCCESS_PAGE_CONTROLLER, new { pageTitle = pageTitle, retAction = action, retController = controller, waitSeconds = waitSeconds });
        }

        #endregion
    }
}