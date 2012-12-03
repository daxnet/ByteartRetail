using ByteartRetail.ServiceContracts;
using ByteartRetail.Infrastructure.Communication;
using ByteartRetail.Infrastructure.Config;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ByteartRetail.Web
{
    public enum ImageSize
    {
        Small,
        Medium,
        Large
    }

    public static class HtmlHelperExtension
    {
        #region ActionLinkWithPermission
        public static MvcHtmlString ActionLinkWithPermission(this HtmlHelper helper, string linkText, string action, string controller, PermissionKeys required)
        {
            if (helper == null ||
                helper.ViewContext == null ||
                helper.ViewContext.RequestContext == null ||
                helper.ViewContext.RequestContext.HttpContext == null ||
                helper.ViewContext.RequestContext.HttpContext.User == null ||
                helper.ViewContext.RequestContext.HttpContext.User.Identity == null)
                return MvcHtmlString.Empty;

            using (ServiceProxy<IUserService> proxy = new ServiceProxy<IUserService>())
            {
                var role = proxy.Channel.GetUserRoleByUserName(helper.ViewContext.RequestContext.HttpContext.User.Identity.Name);
                if (role == null)
                    return MvcHtmlString.Empty;
                var keyName = ByteartRetailConfigurationReader.Instance.GetKeyNameByRoleName(role.Name);
                if (keyName == null)
                    throw new ConfigurationErrorsException(string.Format("在配置文件中没有定义与角色名称 {0} 相对应的权限键（Permission Key）名称。", role.Name));
                var namesInEnum = Enum.GetNames(typeof(PermissionKeys));
                if (!namesInEnum.Contains(keyName))
                    throw new ConfigurationErrorsException(string.Format("在配置文件中对角色名称 {0} 设定的权限键（Permission Key）名称无效。", role.Name));
                var permissionKey = (PermissionKeys)Enum.Parse(typeof(PermissionKeys), keyName);
                if ((permissionKey & required) == permissionKey)
                    return MvcHtmlString.Create(HtmlHelper.GenerateLink(helper.ViewContext.RequestContext, helper.RouteCollection, linkText, null, action, controller, null, null));
                else
                    return MvcHtmlString.Empty;
            }
        }
        #endregion
        #region ImageActionLink
        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imgSrc, string altText, string text, string action, string controller, object routeValues, object htmlAttributes)
        {
            var url = UrlHelper.GenerateUrl(null, action, controller, new RouteValueDictionary(routeValues), helper.RouteCollection, helper.ViewContext.RequestContext, true);
            TagBuilder tbImg = new TagBuilder("img");
            tbImg.MergeAttribute("src", imgSrc);
            tbImg.MergeAttribute("alt", altText);
            tbImg.MergeAttribute("border", "0");
            tbImg.MergeAttribute("title", altText); 
            var imgString = tbImg.ToString(TagRenderMode.SelfClosing);
            TagBuilder tbA = new TagBuilder("a")
            {
                InnerHtml = imgString
            };
            if (!string.IsNullOrEmpty(text))
            {
                tbA.InnerHtml += HttpUtility.HtmlEncode(text);
            }
            tbA.MergeAttribute("href", url);
            if (htmlAttributes != null)
            {
                IDictionary<string, object> additionAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tbA.MergeAttributes<string, object>(additionAttributes);
            }
            return MvcHtmlString.Create(tbA.ToString());
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imgSrc, string altText, string action, string controller, object routeValues, object htmlAttributes)
        {
            return ImageActionLink(helper, imgSrc, altText, null, action, controller, routeValues, htmlAttributes);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imgSrc, string altText, string action, string controller, object routeValues)
        {
            return ImageActionLink(helper, imgSrc, altText, action, controller, routeValues, null);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imgSrc, string altText, string text, string action, string controller, object routeValues)
        {
            return ImageActionLink(helper, imgSrc, altText, text, action, controller, routeValues, null);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imgSrc, string altText, string text, string action, string controller)
        {
            return ImageActionLink(helper, imgSrc, altText, text, action, controller, null);
        }
        #endregion

        #region Image
        public static MvcHtmlString Image(this HtmlHelper helper, string rawFile)
        {
            var imgFile = UrlHelper.GenerateContentUrl(string.Format("~/Images/{0}", rawFile), helper.ViewContext.HttpContext);
            TagBuilder tb = new TagBuilder("img");
            tb.MergeAttribute("src", imgFile);
            tb.MergeAttribute("border", "0");
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
        }
        #endregion

        #region ProductImage
        public static MvcHtmlString ProductImage(this HtmlHelper helper, string rawFile, ImageSize size, bool noCaching, object htmlAttributes)
        {
            var imgSizeIndicator = System.Enum.GetName(typeof(ImageSize), size);
            var imgFile = UrlHelper.GenerateContentUrl(string.Format("~/Images/Products/{0}", rawFile), helper.ViewContext.HttpContext);
            TagBuilder tb = new TagBuilder("img");
            if (noCaching)
                tb.MergeAttribute("src", imgFile + "?" + new Random().NextDouble().ToString());
            else
                tb.MergeAttribute("src", imgFile);
            tb.MergeAttribute("border", "0");
            var sizeValue = 65;
            switch (size)
            {
                case ImageSize.Medium:
                    sizeValue = 130;
                    break;
                case ImageSize.Large:
                    sizeValue = 195;
                    break;
                default:
                    break;
            }
            tb.MergeAttribute("width", sizeValue.ToString());
            tb.MergeAttribute("height", sizeValue.ToString());
            if (htmlAttributes != null)
            {
                IDictionary<string, object> additionAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                tb.MergeAttributes<string, object>(additionAttributes);
            }
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ProductImage(this HtmlHelper helper, string rawFile, bool noCaching, ImageSize size)
        {
            return ProductImage(helper, rawFile, size, noCaching, null);
        }

        public static MvcHtmlString ProductImage(this HtmlHelper helper, string rawFile, ImageSize size, object htmlAttributes)
        {
            return ProductImage(helper, rawFile, size, true, htmlAttributes);
        }

        public static MvcHtmlString ProductImage(this HtmlHelper helper, string rawFile, ImageSize size)
        {
            return ProductImage(helper, rawFile, size, true, null);
        }
        #endregion

        #region ImageSubmitButton
        public static MvcHtmlString ImageSubmitButton(this HtmlHelper helper, string formName, string imgSrc, string altText, string text)
        {
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttribute("alt", altText);
            imgTag.MergeAttribute("border", "0");
            var img = imgTag.ToString(TagRenderMode.SelfClosing);
            TagBuilder aTag = new TagBuilder("a")
            {
                InnerHtml = img + HttpUtility.HtmlEncode(text)
            };
            aTag.MergeAttribute("style", "cursor: pointer");
            aTag.MergeAttribute("onclick", string.Format("javascript: $('#{0}').submit()", formName));
            return MvcHtmlString.Create(aTag.ToString());
        }
        #endregion
    }
}