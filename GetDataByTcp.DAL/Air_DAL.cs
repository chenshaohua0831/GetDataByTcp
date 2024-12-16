using Dapper;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.DAL
{
    public class Air_DAL
    {
        public int Insert_Air(Air_Model model,string tableName)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $@"INSERT INTO {tableName} (
	                        StationCode,
	                        TimePoint,
	                        [value],
	                        operationTime
                        )
                        VALUES
	                        (
		                        @StationCode,
		                        @TimePoint,
		                        @Value,
		                        @operationTime
	                        )";
                return db.Execute(sql,model);
            }
        }
        /// <summary>
        /// 获取5分钟值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Air_Model> GetAir5mListByQuery(Air_Model_Query query, string tableName)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $"select a.*,b.positionname from {tableName}  a inner join sys_station b on a.stationcode =b.stationcode  where 1=1";
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    sql += $" and a.timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    sql += $" and a.timepoint<='{query.TimePointEnd}'";
                }
                sql += "order by a.id desc";
                return db.Query<Air_Model>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取小时值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Air_Model> GetAirhListByQuery(Air_Model_Query query, string tableName)
        {
            string sql = $"select a.*,b.positionname from {tableName}  a inner join sys_station b on a.stationcode =b.stationcode where 1=1";
            using (var db = SQLHelper.OpenConnection())
            {
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    sql += $" and a.timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    sql += $" and a.timepoint<='{query.TimePointEnd}'";
                }
                if (!string.IsNullOrWhiteSpace(query.StationCode))
                {
                    sql += $" and a.StationCode='{query.StationCode}'";
                }
                if (query.StationCodeList.Count>0)
                {
                    string str = string.Join(",", query.StationCodeList.Select(s => "'" + s + "'").ToArray());
                    sql += $" and a.StationCode in ({str})";
                }
                sql += "order by a.id desc";
                return db.Query<Air_Model>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取日值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Air_Model> GetAirDayListByQuery(Air_Model_Query query)
        {
            string sql = $"select a.*,b.positionname from Air_day_pm25_Src a inner join sys_station b on a.stationcode =b.stationcode  where 1=1";
            using (var db = SQLHelper.OpenConnection())
            {
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    sql += $" and a.timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    sql += $" and a.timepoint<='{query.TimePointEnd}'";
                }
                if (!string.IsNullOrWhiteSpace(query.StationCode))
                {
                    sql += $" and a.StationCode='{query.StationCode}'";
                }
                if (query.StationCodeList.Count > 0)
                {
                    string str = string.Join(",", query.StationCodeList.Select(s => "'" + s + "'").ToArray());
                    sql += $" and a.StationCode in ({str})";
                }
                sql += "order by a.id desc";
                return db.Query<Air_Model>(sql).ToList();
            }
        }


        /// <summary>
        /// 获取5分钟值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAir5mPageListByQuery(Air_Model_Query query, string tableName, int pageIndex, int pageSize)
        {
            using (var db = SQLHelper.OpenConnection())
            {

                string where = "";
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    where += $" and timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    where += $" and timepoint<='{query.TimePointEnd}'";
                }


                string sql = $@"WITH OrderedTable AS (
                                                                SELECT a.*,b.positionname, ROW_NUMBER() OVER (ORDER BY id desc) AS RowNumber
                                                                FROM {tableName} a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}
                                                            )
                                                            SELECT *
                                                            FROM OrderedTable
                                                            WHERE RowNumber BETWEEN({pageIndex} - 1) * {pageSize} + 1 AND {pageIndex} *{pageSize}";
                string countSql = $@"SELECT count(*)
                                                                FROM {tableName} a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}";

                PageList<Air_Model> pageList = new PageList<Air_Model>();
                pageList.DataList = db.Query<Air_Model>(sql).ToList();
                pageList.Count = db.ExecuteScalar<int>(countSql);
                return pageList;
            }
        }

        /// <summary>
        /// 获取小时值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAirHourPageListByQuery(Air_Model_Query query, string tableName, int pageIndex, int pageSize)
        {
            using (var db = SQLHelper.OpenConnection())
            {

                string where = "";
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    where += $" and a.timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    where += $" and a.timepoint<='{query.TimePointEnd}'";
                }

                if (!string.IsNullOrWhiteSpace(query.StationCode))
                {
                    where += $" and a.StationCode='{query.StationCode}'";
                }

                string sql = $@"WITH OrderedTable AS (
                                                                SELECT a.*,b.positionname, ROW_NUMBER() OVER (ORDER BY id desc) AS RowNumber
                                                                FROM {tableName} a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}
                                                            )
                                                            SELECT *
                                                            FROM OrderedTable
                                                            WHERE RowNumber BETWEEN({pageIndex} - 1) * {pageSize} + 1 AND {pageIndex} *{pageSize}";
                string countSql = $@"SELECT count(*)
                                                                FROM {tableName} a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}";

                PageList<Air_Model> pageList = new PageList<Air_Model>();
                pageList.DataList = db.Query<Air_Model>(sql).ToList();
                pageList.Count = db.ExecuteScalar<int>(countSql);
                return pageList;
            }
        }
        /// <summary>
        /// 获取日值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAirDayPageListByQuery(Air_Model_Query query, int pageIndex, int pageSize)
        {
            using (var db = SQLHelper.OpenConnection())
            {

                string where = "";
                if (!string.IsNullOrWhiteSpace(query.TimePointStart))
                {
                    where += $" and a.timepoint>='{query.TimePointStart}'";
                }
                if (!string.IsNullOrWhiteSpace(query.TimePointEnd))
                {
                    where += $" and a.timepoint<='{query.TimePointEnd}'";
                }

                if (!string.IsNullOrWhiteSpace(query.StationCode))
                {
                    where += $" and a.StationCode='{query.StationCode}'";
                }

                string sql = $@"WITH OrderedTable AS (
                                                                SELECT a.*,b.positionname, ROW_NUMBER() OVER (ORDER BY id desc) AS RowNumber
                                                                FROM Air_day_pm25_Src a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}
                                                            )
                                                            SELECT *
                                                            FROM OrderedTable
                                                            WHERE RowNumber BETWEEN({pageIndex} - 1) * {pageSize} + 1 AND {pageIndex} *{pageSize}";
                string countSql = $@"SELECT count(*)
                                                                FROM Air_day_pm25_Src a inner join sys_station b on a.stationcode =b.stationcode
                                                                where 1=1 {where}";

                PageList<Air_Model> pageList = new PageList<Air_Model>();
                pageList.DataList = db.Query<Air_Model>(sql).ToList();
                pageList.Count = db.ExecuteScalar<int>(countSql);
                return pageList;
            }
        }
        public Air_Model GetSingleAirHourData(string timePoint, string stationCode,string tableName)
        {
            string sql = $@"select a.*,b.positionname from {tableName}  a inner join sys_station b on a.stationcode =b.stationcode where 1=1
                                    and a.timepoint ='{timePoint}' and a.stationcode = {stationCode}";
            using (var db = SQLHelper.OpenConnection())
            {
                return db.Query<Air_Model>(sql).FirstOrDefault();
            }
        }
        public int InsertAirHourUpdate(Air_Model model)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $@"INSERT INTO Air_h_pm25_update (
	                                        StationCode,
	                                        TimePoint,
	                                        ValueStr,
	                                        operationTime,
	                                        DataStatus,
	                                        Operator
                                        )
                                        VALUES
	                                        (
		                                        @StationCode,
		                                        @TimePoint,
		                                        @ValueStr,
		                                        GETDATE(),
		                                        @DataStatus,
		                                        @Operator
	                                        )";
                return db.Execute(sql, model);
            }
        }
        public int UpdateAirHourUpdate(Air_Model model)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $@"update Air_h_pm25_update set
	                                        ValueStr = @ValueStr,
	                                        DataStatus = @DataStatus,
	                                        Operator = @Operator
                                        where StationCode = @StationCode and TimePoint=@TimePoint";
                return db.Execute(sql, model);
            }
        }
        public int InsertAirFinishDay(DateTime finishDate)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $@"INSERT INTO Air_pm25_finish_day (
	                                        FinishDate
                                        )
                                        VALUES
	                                        (
		                                        @finishDate
	                                        )";
                return db.Execute(sql, new { finishDate });
            }
        }
        public int DeleteAirFinishDay(string finishDate)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = $@"delete from Air_pm25_finish_day where FinishDate ='{finishDate}'";
                return db.Execute(sql);
            }
        }
        public bool ExistAirFinishDay(string finishDate)
        {
            string sql = $@"select count(0) from Air_pm25_finish_day where  FinishDate ='{finishDate}'";
            using (var db = SQLHelper.OpenConnection())
            {
                var count = db.ExecuteScalar<int>(sql);
                return count > 0;
            }
        }
    }
}
