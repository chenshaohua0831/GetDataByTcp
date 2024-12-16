using GetDataByTcp.DAL;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.BLL
{
    public class Air_RealTime_PM25_BLL
    {
        public List<Air_RealTime_PM25_Model> GetAirRealTimePM25ListByQuery(string machineNum, string dateStart, string dateEnd)
        {
            Air_RealTime_PM25_DAL dal = new Air_RealTime_PM25_DAL();
            return dal.GetAirRealTimePM25ListByQuery(machineNum, dateStart, dateEnd);
        }
    }
}
