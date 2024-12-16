using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Model
{
    public class Air_Model_Query
    {
        public string StationCode { get; set; }
        public string TimePointStart { get; set; }
        public string TimePointEnd { get; set; }
        public List<string> StationCodeList { get; set; } =new List<string>();
    }
}
