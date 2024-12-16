using Dapper;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.DAL
{
    public class Station_DAL
    {
        public List<Station_Model> GetAllStationList()
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "select * from sys_station order by stationcode";
                return db.Query<Station_Model>(sql).ToList();
            }
        }
    }
}
