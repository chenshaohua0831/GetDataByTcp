using GetDataByTcp.DAL;
using GetDataByTcp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.BLL
{
    public class SysUserBLL
    {
        public Sys_User Login(string usercode,string pwd)
        { 
            SysUserDAL dal = new SysUserDAL();
            return dal.Login(usercode,pwd);
        }
        public int UpdateLastLoginTime(Sys_User user)
        {
            SysUserDAL dal = new SysUserDAL();
            return dal.UpdateLastLoginTime(user);
        }
        public PageList<Sys_User> GetSysUserPageList(Sys_User_Query query)
        {
            SysUserDAL dal = new SysUserDAL();
            return dal.GetSysUserPageList(query);
        }
        public int AddSysUser(Sys_User user)
        {
            SysUserDAL dal = new SysUserDAL();
            return dal.AddSysUser(user);
        }
        public int UpdateSysUser(Sys_User user)
        {
            SysUserDAL dal = new SysUserDAL();
            return dal.UpdateSysUser(user);
        }
        public int UpdateEnabled(Sys_User user)
        {
            SysUserDAL dal = new SysUserDAL();
            return dal.UpdateEnabled(user);
        }
    }
}
