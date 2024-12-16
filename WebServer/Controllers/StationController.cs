using GetDataByTcp.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.filter;

namespace WebServer.Controllers
{
    public class StationController : Controller
    {
        [TkAuth(Disable = true)]
        public JsonResult GetAllStationList()
        {
            Station_BLL bll= new Station_BLL();
            var result = bll.GetAllStationList();
            return Json(new { error_code = 0, msg = "操作成功",data = result });
        }
    }
}