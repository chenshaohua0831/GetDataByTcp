using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Model
{
    public class Air_RealTime_PM25_Model
    {
        public string MachineNum { get; set; }
        public DateTime TimePoint { get; set; }
        public decimal Value { get; set; }
        public string Flag { get; set; }
        public DateTime OperationTime { get; set; }
        public string Mark { get; set; }
    }
}
