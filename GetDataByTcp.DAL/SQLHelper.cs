using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace GetDataByTcp.DAL
{
    public class SQLHelper
    {
        //// 打开数据库连接
        //public static IDbConnection OpenConnection()
        //{
        //    IDbConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString);
        //    connection.Open();
        //    return connection;
        //}
        //// 打开数据库连接
        public static IDbConnection OpenConnection()
        {
            IDbConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServerConnection"].ConnectionString);
            connection.Open();
            return connection;
        }
        // 查询单个对象
        public T Get<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, parameters);
            }
        }

        // 查询对象列表
        public List<T> GetList<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<T>(sql, parameters).AsList();
            }
        }

        // 执行插入操作并返回插入的自增ID
        public int Insert(string sql, object parameters)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.ExecuteScalar<int>(sql, parameters);
            }
        }

        // 执行更新操作
        public int Update(string sql, object parameters)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }

        // 执行删除操作
        public int Delete(string sql, object parameters)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Execute(sql, parameters);
            }
        }
    }
}