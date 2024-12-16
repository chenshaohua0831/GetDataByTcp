using GetDataByTcp.BLL;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.Common;
using WebServer.filter;

namespace WebServer.Controllers
{
    [TkAuth(Disable = true)]
    public class SysUserController : Controller
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult Login(string usercode,string pwd)
        {
            SysUserBLL bll= new SysUserBLL();
            var user = bll.Login(usercode,pwd);
            if (user==null)
            {
                return Json(new { error_code = -1, msg = "账号或密码错误，登录失败" });
            }
            if (user.Enabled == 1)//是否启用
            {
                user.LastLoginTime = DateTime.Now;
                //更新登录时间
                bll.UpdateLastLoginTime(user);
                var token = DESHelper.EncryptString(Newtonsoft.Json.JsonConvert.SerializeObject(user));
                return Json(new { error_code = 0, msg = "登录成功", data = new { id = user.Id, username = user.UserName, token } });
            }
            else
            {
                return Json(new { error_code = -1, msg = "该用户未开通权限，请联系管理员" });
            }
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetSysUserPageList(Sys_User_Query query)
        {
            try
            {
                SysUserBLL bll = new SysUserBLL();
                var result = bll.GetSysUserPageList(query);
                return Json(new { error_code = 0, msg = "操作成功", total_count = result.Count, data = result.DataList });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = "操作失败，请联系管理员", error = ex.ToString() });
            }
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult AddSysUser(Sys_User user)
        {
            try
            {
                SysUserBLL bll = new SysUserBLL();
                var result = bll.AddSysUser(user);
                return Json(new { error_code = 0, msg = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = "操作失败", error = ex.ToString() });
            }
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JsonResult UpdateSysUser(Sys_User user)
        {
            try
            {
                SysUserBLL bll = new SysUserBLL();
                var result = bll.UpdateSysUser(user);
                return Json(new { error_code = 0, msg = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = "操作失败", error = ex.ToString() });
            }
        }
        public JsonResult UpdateEnabled(Sys_User user)
        {
            try
            {
                SysUserBLL bll = new SysUserBLL();
                var result = bll.UpdateEnabled(user);
                return Json(new { error_code = 0, msg = "操作成功" });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = "操作失败", error = ex.ToString() });
            }
        }
    }
}