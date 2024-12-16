using GetDataByTcp.DAL;
using GetDataByTcp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.BLL
{
    public class Station_BLL
    {
        public List<Station_Model> GetAllStationList()
        { 
            Station_DAL dal = new Station_DAL();
            return dal.GetAllStationList();
        }
    }
}
