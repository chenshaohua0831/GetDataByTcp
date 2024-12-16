using Dapper;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.DAL
{
    public class SysUserDAL
    {
        public Sys_User Login(string usercode,string pwd)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "select * from sys_ruite_user where usercode=@usercode and pwd=@pwd";
                return db.Query<Sys_User>(sql, new { usercode , pwd }).FirstOrDefault();
            }
        }
        public int UpdateLastLoginTime(Sys_User user)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "update sys_ruite_user set lastlogintime=@LastLoginTime where id=@Id";
                return db.Execute(sql, user);
            }
        }
        public PageList<Sys_User> GetSysUserPageList(Sys_User_Query query)
        {
            using (var db = SQLHelper.OpenConnection())
            {

                string where = "";
                if (!string.IsNullOrWhiteSpace(query.UserName))
                {
                    where += $" and a.UserName like '%{query.UserName}%'";
                }
                if (!string.IsNullOrWhiteSpace(query.UserCode))
                {
                    where += $" and a.UserCode like '%{query.UserCode}%'";
                }


                string sql = $@"WITH TableList AS (
                                                                SELECT a.* ROW_NUMBER() OVER (ORDER BY id desc) AS RowNumber
                                                                FROM sys_ruite_user a 
                                                                where 1=1 {where}
                                                            )
                                                            SELECT *
                                                            FROM TableList
                                                            WHERE RowNumber BETWEEN({query.PageIndex} - 1) * {query.PageSize} + 1 AND {query.PageIndex} *{query.PageSize}";
                string countSql = $@"SELECT count(*)
                                                                FROM sys_ruite_user a i
                                                                where 1=1 {where}";

                PageList<Sys_User> pageList = new PageList<Sys_User>();
                pageList.DataList = db.Query<Sys_User>(sql).ToList();
                pageList.Count = db.ExecuteScalar<int>(countSql);
                return pageList;
            }
        }
        public int AddSysUser(Sys_User user)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = @"INSERT INTO Sys_Ruite_User (
	                                            UserCode,
	                                            PWD,
	                                            UserName,
	                                            LastLoginTime,
	                                            Enabled,
	                                            Remark
                                            )
                                            VALUES
                                            (
	                                            @UserCode,
	                                            @PWD,
	                                            @UserName,
	                                            GETDATE(),
	                                            @Enabled,
	                                            @Remark
                                            )";
                return db.Execute(sql, user);
            }
        }
        public int UpdateSysUser(Sys_User user)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = @"update Sys_Ruite_User set
	                                            UserName = @UserName,
	                                            Enabled=@Enabled,
	                                            Remark=@Remark
                                            where id = @Id";
                return db.Execute(sql, user);
            }
        }
        public int UpdateEnabled(Sys_User user)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "update sys_ruite_user set Enabled=@Enabled where id=@Id";
                return db.Execute(sql, user);
            }
        }
    }
}
