using GetDataByTcp.DAL;
using GetDataByTcp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GetDataByTcp.BLL
{
    public class Air_BLL
    {
        public int Insert_Air(Air_Model model,string tableName)
        {
            Air_DAL dal = new Air_DAL();
            return dal.Insert_Air(model, tableName);
        }
        public void Insert_Air_5m_Job(string dateStart,string dateEnd)
        {
            Air_DAL air_5m_DAL = new Air_DAL();
            Station_DAL station_DAL = new Station_DAL();
            Air_RealTime_PM25_DAL air_RealTime_PM25_DAL = new Air_RealTime_PM25_DAL();
            var airRealTimeList = air_RealTime_PM25_DAL.GetAirRealTimePM25ListByDateBetween(dateStart, dateEnd);
            var stationList = station_DAL.GetAllStationList();
            foreach (var station in stationList)
            {
                try
                {
                    //if (station.UniqueCode== "SYDM5018210112004")
                    //{
                    var stationAirRealTimeList = airRealTimeList.Where(s => s.MachineNum == station.UniqueCode).ToList();
                    if (stationAirRealTimeList.Count()>0)
                    {
                        Air_Model model = new Air_Model();
                        model.StationCode = station.StationCode;
                        model.TimePoint = Convert.ToDateTime(dateStart);
                        model.Value = Math.Round(stationAirRealTimeList.Average(s => s.Value), 0);
                        model.OperationTime = DateTime.Now;
                        string tableName = $"Air_5m_{DateTime.Now.ToString("yyyy")}_{station.StationCode}A_Src";
                        air_5m_DAL.Insert_Air(model, tableName);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 获取5分钟值的缺少数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public List<Air_Model> GetAir5mNullDataList(Air_Model_Query query, string tableName, string stationCode)
        {
            var nullDataList = new List<Air_Model>();
            Air_DAL air_DAL = new Air_DAL();
            var dataList = air_DAL.GetAir5mListByQuery(query, tableName);
            var dateStart = Convert.ToDateTime(query.TimePointStart);
            var dateEnd = Convert.ToDateTime(query.TimePointEnd);
            while (dateEnd > dateStart && dateStart <= DateTime.Now.AddMinutes(-5))
            {
                var count = dataList.Where(s => s.TimePoint == dateStart).Count();
                if (count == 0)
                {
                    Air_Model model = new Air_Model();
                    model.StationCode = stationCode;
                    model.TimePoint = dateStart;
                    nullDataList.Add(model);
                }
                dateStart = dateStart.AddMinutes(5);
            }



            return nullDataList;
        }
        /// <summary>
        /// 获取小时值的缺少数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<Air_Model> GetAirhNullDataList(Air_Model_Query query, string tableName)
        {
            var nullDataList = new List<Air_Model>();
            Air_DAL air_DAL = new Air_DAL();
            var dataList = air_DAL.GetAirhListByQuery(query, tableName);
            var dateStart = Convert.ToDateTime(query.TimePointStart);
            var dateEnd = Convert.ToDateTime(query.TimePointEnd);
            while (dateEnd > dateStart && dateStart <= DateTime.Now.AddHours(-1))
            {
                for (int i = 0; i < query.StationCodeList.Count; i++)
                {
                    var count = dataList.Where(s => s.TimePoint == dateStart && s.StationCode == query.StationCodeList[i]).Count();
                    if (count == 0)
                    {
                        Air_Model model = new Air_Model();
                        model.StationCode = query.StationCodeList[i];
                        model.TimePoint = dateStart;
                        nullDataList.Add(model);
                    }
                }
                dateStart = dateStart.AddHours(1);
            }



            return nullDataList;
        }
        /// <summary>
        /// 获取日值的缺少数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Air_Model> GetAirDayNullDataList(Air_Model_Query query)
        {
            var nullDataList = new List<Air_Model>();
            Air_DAL air_DAL = new Air_DAL();
            var dataList = air_DAL.GetAirDayListByQuery(query);
            var dateStart = Convert.ToDateTime(query.TimePointStart);
            var dateEnd = Convert.ToDateTime(query.TimePointEnd);
            while (dateEnd > dateStart && dateStart <= DateTime.Now.AddDays(-1))
            {
                for (int i = 0; i < query.StationCodeList.Count; i++)
                {
                    var count = dataList.Where(s => s.TimePoint == dateStart && s.StationCode == query.StationCodeList[i]).Count();
                    if (count == 0)
                    {
                        Air_Model model = new Air_Model();
                        model.StationCode = query.StationCodeList[i];
                        model.TimePoint = dateStart;
                        nullDataList.Add(model);
                    }
                }
                dateStart = dateStart.AddDays(1);
            }



            return nullDataList;
        }


        /// <summary>
        /// 获取5分钟值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAir5mPageListByQuery(Air_Model_Query query, string tableName, int pageIndex, int pageSize)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAir5mPageListByQuery(query, tableName, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取小时值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAirHourPageListByQuery(Air_Model_Query query, string tableName, int pageIndex, int pageSize)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAirHourPageListByQuery(query, tableName, pageIndex, pageSize);
        }
        /// <summary>
        /// 获取日值分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public PageList<Air_Model> GetAirDayPageListByQuery(Air_Model_Query query, int pageIndex, int pageSize)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAirDayPageListByQuery(query, pageIndex, pageSize);
        }
        public List<Air_Model> GetAir5mListByQuery(Air_Model_Query query, string tableName)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAir5mListByQuery(query, tableName);
        }
        public List<Air_Model> GetAirhListByQuery(Air_Model_Query query, string tableName)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAirhListByQuery(query, tableName);
        }
        public List<Air_Model> GetAirDayListByQuery(Air_Model_Query query)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetAirDayListByQuery(query);
        }
        public Air_Model GetSingleAirHourData(string timePoint, string stationCode, string tableName)
        {
            Air_DAL dal = new Air_DAL();
            return dal.GetSingleAirHourData(timePoint, stationCode, tableName);
        }
        public int InsertAirHourUpdate(Air_Model model)
        {
            Air_DAL dal = new Air_DAL();
            return dal.InsertAirHourUpdate(model);
        }
        public int UpdateAirHourUpdate(Air_Model model)
        {
            Air_DAL dal = new Air_DAL();
            return dal.UpdateAirHourUpdate(model);
        }
        public int InsertAirFinishDay(DateTime finishDate)
        {
            Air_DAL dal = new Air_DAL();
            return dal.InsertAirFinishDay(finishDate);
        }
        public int DeleteAirFinishDay(string finishDate)
        {
            Air_DAL dal = new Air_DAL();
            return dal.DeleteAirFinishDay(finishDate);
        }
        public bool ExistAirFinishDay(string finishDate)
        {
            Air_DAL dal = new Air_DAL();
            return dal.ExistAirFinishDay(finishDate);
        }
    }
}
