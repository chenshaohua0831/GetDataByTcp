using Dapper;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.DAL
{
    public class Air_RealTime_PM25_DAL
    {
        public List<Air_RealTime_PM25_Model> GetAirRealTimePM25ListByDateBetween(string dateStart,string dateEnd)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "select * from Air_RealTime_PM25_Src t where t.TimePoint>=@dateStart and t.TimePoint<@dateEnd";
                return db.Query<Air_RealTime_PM25_Model>(sql, new { dateStart , dateEnd }).ToList();
            }
        }
        public List<Air_RealTime_PM25_Model> GetAirRealTimePM25ListByQuery(string machineNum , string dateStart, string dateEnd)
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "select * from Air_RealTime_PM25_Src t where t.machineNum=@machineNum and t.TimePoint>=@dateStart and t.TimePoint<@dateEnd";
                return db.Query<Air_RealTime_PM25_Model>(sql, new { dateStart, dateEnd, machineNum }).ToList();
            }
        }
    }
}
