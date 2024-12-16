using GetDataByTcp.Model;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WebServer.Common;

namespace WebServer.filter
{

    public class TkAuth : ActionFilterAttribute
    {
        public bool Disable { get; set; } = false;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Disable) return;

            base.OnActionExecuting(filterContext);
            var token = filterContext.HttpContext.Request.Headers["token"];

            try
            {
                if (string.IsNullOrWhiteSpace(token))//没有token
                {
                    var response = new { error_code = -1, msg = "token不存在" };
                    filterContext.Result = new JsonResult()
                    {
                        Data = response
                    };
                    return;
                }
                var userinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Sys_User>(DESHelper.DecryptString(token));
                if (userinfo == null)
                {
                    var response = new { error_code = -1, msg = "token不合法" };
                    filterContext.Result = new JsonResult()
                    {
                        Data = response
                    };
                    return;
                }
                else
                {
                    if (DateTime.Now > userinfo.LastLoginTime.AddDays(7))//token时间7天
                    {
                        var response = new { error_code = -1, msg = "token超时" };
                        filterContext.Result = new JsonResult()
                        {
                            Data = response
                        };
                        return;
                    }
                    //if (Sysuser.FindByid(userinfo.id) == null)//判断用户是否存在
                    //{
                    //    var response = new { error_code = -1, msg = "用户不存在" };
                    //    filterContext.Result = new JsonResult()
                    //    {
                    //        Data = response
                    //    };
                    //    return;
                    //}
                }

            }
            catch (Exception)
            {
                var response = new { error_code = -1, msg = "接口请求异常，请联系管理员" };
                filterContext.Result = new JsonResult()
                {
                    Data = response
                };
                return;
            }


        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

        }

    }
}