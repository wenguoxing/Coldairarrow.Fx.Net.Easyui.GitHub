﻿using Coldairarrow.Business.Common;
using Coldairarrow.Util;
using System;
using System.Text;
using System.Web.Mvc;

namespace Coldairarrow.Web
{
    /// <summary>
    /// 校验登录
    /// </summary>
    public class CheckLoginAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Action执行之前执行
        /// </summary>
        /// <param name="filterContext">过滤器上下文</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                //若为本地测试，则不需要登录
                if (GlobalSwitch.RunModel == RunModel.LocalTest)
                {
                    return;
                }
                //判断是否需要登录
                bool needLogin = filterContext.ContainsAttribute<CheckLoginAttribute>() && !filterContext.ContainsAttribute<IgnoreLoginAttribute>();

                //转到登录
                if (needLogin && !Operator.Logged())
                {
                    RedirectToLogin();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                BusHelper.HandleException(ex);
                RedirectToLogin();
            }

            void RedirectToLogin()
            {
                UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                string loginUrl = urlHelper.Content("~/Home/Login");
                string script = $@"    
<html>
    <script>
        top.location.href = '{loginUrl}';
    </script>
</html>
";
                filterContext.Result = new ContentResult { Content = script, ContentType = "text/html", ContentEncoding = Encoding.UTF8 };
            }
        }

        /// <summary>
        /// Action执行完毕之后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}