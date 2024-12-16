using GetDataByTcp.BLL;
using GetDataByTcp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.XPath;
using WebServer.filter;

namespace WebServer.Controllers
{
    public class PM25Controller : Controller
    {
        [TkAuth(Disable = true)]
        public JsonResult GetAir5mPageListByQuery(Air_Model_Query query, int pageIndex, int pageSize)
        {
            if (Convert.ToDateTime(query.TimePointStart).Year!= Convert.ToDateTime(query.TimePointEnd).Year)
            {
                return Json(new { error_code = -1, msg = "起始结束时间不允许跨年度" });
            }
            Air_BLL bll= new Air_BLL();
            string tableName = $"Air_5m_{Convert.ToDateTime(query.TimePointStart).Year.ToString()}_{query.StationCode}A_Src";

            var result = bll.GetAir5mPageListByQuery(query,tableName,pageIndex,pageSize);
            return Json(new { error_code = 0, msg = "操作成功", total_count = result.Count, data = result.DataList });
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAirHourPageListByQuery(Air_Model_Query query, int pageIndex, int pageSize)
        {
            if (Convert.ToDateTime(query.TimePointStart).Year != Convert.ToDateTime(query.TimePointEnd).Year)
            {
                return Json(new { error_code = -1, msg = "起始结束时间不允许跨年度" });
            }
            Air_BLL bll = new Air_BLL();
            string tableName = $"Air_h_{Convert.ToDateTime(query.TimePointStart).Year.ToString()}_pm25_Src";

            var result = bll.GetAirHourPageListByQuery(query, tableName, pageIndex, pageSize);
            return Json(new { error_code = 0, msg = "操作成功", total_count = result.Count, data = result.DataList });
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAirDayPageListByQuery(Air_Model_Query query, int pageIndex, int pageSize)
        {
            Air_BLL bll = new Air_BLL();

            var result = bll.GetAirDayPageListByQuery(query, pageIndex, pageSize);
            return Json(new { error_code = 0, msg = "操作成功", total_count = result.Count, data = result.DataList });
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAllAir5mListByQuery(Air_Model_Query query)
        {
            try
            {
                if (Convert.ToDateTime(query.TimePointStart).Year != Convert.ToDateTime(query.TimePointEnd).Year)
                {
                    return Json(new { error_code = -1, msg = "起始结束时间不允许跨年度" });
                }
                Station_BLL stationBll = new Station_BLL();
                var stationList = stationBll.GetAllStationList();
                List<Air_Model> result = new List<Air_Model>();
                Air_BLL bll = new Air_BLL();
                for (int i = 0; i < stationList.Count(); i++)
                {
                    string tableName = $"Air_5m_{Convert.ToDateTime(query.TimePointStart).Year.ToString()}_{stationList[i].StationCode}A_Src";
                    var list = bll.GetAir5mListByQuery(query, tableName);
                    result.AddRange(list);
                }

                return Json(new { error_code = 0, msg = "操作成功", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = ex.ToString() });
            }
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAllAirHourListByQuery(Air_Model_Query query)
        {
            try
            {
                if (Convert.ToDateTime(query.TimePointStart).Year != Convert.ToDateTime(query.TimePointEnd).Year)
                {
                    return Json(new { error_code = -1, msg = "起始结束时间不允许跨年度" });
                }
                Air_BLL bll = new Air_BLL();
                string tableName = $"Air_h_{Convert.ToDateTime(query.TimePointStart).Year.ToString()}_pm25_Src";
                var result = bll.GetAirhListByQuery(query, tableName);
                return Json(new { error_code = 0, msg = "操作成功", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = ex.ToString() });
            }
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAllAirDayListByQuery(Air_Model_Query query)
        {
            try
            {
                if (Convert.ToDateTime(query.TimePointStart).Year != Convert.ToDateTime(query.TimePointEnd).Year)
                {
                    return Json(new { error_code = -1, msg = "起始结束时间不允许跨年度" });
                }
                Air_BLL bll = new Air_BLL();
                var result = bll.GetAirDayListByQuery(query);
                return Json(new { error_code = 0, msg = "操作成功", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = ex.ToString() });
            }
        }
        [TkAuth(Disable = true)]
        public JsonResult GetAirHourListByDate(string date)
        {
            try
            {
                Station_BLL stationBll = new Station_BLL();
                var stationList = stationBll.GetAllStationList();
                Air_BLL bll = new Air_BLL();
                string tableName = $"Air_h_{Convert.ToDateTime(date).Year.ToString()}_pm25_Src";
                Air_Model_Query query = new Air_Model_Query();
                query.TimePointStart = date.Trim() + " 00:00:00";
                query.TimePointEnd = date.Trim() + " 23:59:59";
                var dataList = bll.GetAirhListByQuery(query, tableName);
                var updateDataList = bll.GetAirhListByQuery(query, "Air_h_pm25_update");

                List<Air_Model> result = new List<Air_Model>();
                for (int i = 0; i < stationList.Count(); i++)
                {
                    for (int k = 0; k < 24; k++)
                    {
                        Air_Model model = new Air_Model();
                        model.PositionName = stationList[i].PositionName;
                        //判断是否人工修改过
                        var updateModel = updateDataList.Where(s => s.TimePoint == Convert.ToDateTime(query.TimePointStart).AddHours(k) && s.StationCode == stationList[i].StationCode).FirstOrDefault();
                        if (updateModel != null)
                        {
                            model.IsUpate = 1;
                            model.StationCode=updateModel.StationCode;
                            model.ValueStr= updateModel.ValueStr;
                            model.TimePoint=updateModel.TimePoint;
                            model.OperationTime= updateModel.OperationTime;
                            model.DataStatus = updateModel.DataStatus;
                            model.Operator= updateModel.Operator;
                        }
                        else //未人工修改
                        {
                            var dataListByStation = dataList.Where(s =>s.StationCode == stationList[i].StationCode).ToList();
                            var search = dataListByStation.Where(s => s.TimePoint == Convert.ToDateTime(query.TimePointStart).AddHours(k)).FirstOrDefault();
                            //判断是否存在小时数据
                            if (search != null)
                            {
                                model.Id = search.Id;
                                model.StationCode = search.StationCode;
                                model.Value = search.Value;
                                model.ValueStr = search.Value.ToString();
                                model.TimePoint = search.TimePoint;
                                model.OperationTime = search.OperationTime;

                                //判断是否为0值
                                if (model.Value == 0)
                                {
                                    model.DataStatus = 3;
                                }
                                //判断是否为负值
                                if (model.Value < 0)
                                {
                                    model.DataStatus = 2;
                                }
                                //判断是否为3小时恒值
                                if (k>=3)
                                {
                                    var temp1 = dataListByStation.Where(s => s.TimePoint == Convert.ToDateTime(query.TimePointStart).AddHours(k-1)).FirstOrDefault();
                                    if (temp1!=null)
                                    {
                                        if (temp1.Value==model.Value)
                                        {
                                            var temp2 = dataListByStation.Where(s => s.TimePoint == Convert.ToDateTime(query.TimePointStart).AddHours(k - 2)).FirstOrDefault();
                                            if (temp2!=null)
                                            {
                                                if (temp2.Value==temp1.Value)
                                                {
                                                    model.DataStatus = 1;
                                                    if (result[result.Count - 1].IsUpate == 0)
                                                    {
                                                        result[result.Count - 1].DataStatus = 1;
                                                    }
                                                    if (result[result.Count - 2].IsUpate == 0)
                                                    {
                                                        result[result.Count - 2].DataStatus = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else //不存在小时数据
                            {
                                model.StationCode = stationList[i].StationCode;
                                model.TimePoint = Convert.ToDateTime(query.TimePointStart).AddHours(k);
                                model.OperationTime = DateTime.Now;
                                model.DataStatus = 4;
                            }
                        }
                        result.Add(model);


                        

                    }
                }


                return Json(new { error_code = 0, msg = "操作成功", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { error_code = -1, msg = ex.ToString() });
            }
        }
        [TkAuth(Disable = true)]
        public JsonResult GetSingleAirHourData(string timePoint,string stationCode)
        {
            Air_BLL bll = new Air_BLL();
            string tableName = $"Air_h_{Convert.ToDateTime(timePoint).Year.ToString()}_pm25_Src";
            var old_data = bll.GetSingleAirHourData(timePoint, stationCode, tableName);
            if (old_data!=null)
            {
                old_data.ValueStr = old_data.Value.ToString();
            }
            var new_data = bll.GetSingleAirHourData(timePoint, stationCode, "Air_h_pm25_update");
            return Json(new { error_code = 0, msg = "操作成功", data = new { old_data , new_data } });
        }
        [TkAuth(Disable = true)]
        public JsonResult SaveNewAirHourData(Air_Model model)
        {
            Air_BLL bll = new Air_BLL();
            var search = bll.GetSingleAirHourData(model.TimePoint.ToString("yyyy-MM-dd HH:mm:ss"), model.StationCode, "Air_h_pm25_update");
            if (search!=null)
            {
                bll.UpdateAirHourUpdate(model);
            }
            else
            {
                bll.InsertAirHourUpdate(model);
            }
            return Json(new { error_code = 0, msg = "操作成功"});
        }
        [TkAuth(Disable = true)]
        public JsonResult InsertAirFinishDay(string finishDate)
        {
            Air_BLL bll = new Air_BLL();
            var exist = bll.ExistAirFinishDay(finishDate);
            if (exist)
            {
                return Json(new { error_code = -1, msg = "当前日期已审核，无法重复审核" });
            }
            else
            {
                bll.InsertAirFinishDay(Convert.ToDateTime(finishDate));
                return Json(new { error_code = 0, msg = "操作成功" });
            }
        }
        [TkAuth(Disable = true)]
        public JsonResult DeleteAirFinishDay(string finishDate)
        {
            Air_BLL bll = new Air_BLL();
            bll.DeleteAirFinishDay(finishDate);
            return Json(new { error_code = 0, msg = "操作成功" });
        }
    }
}