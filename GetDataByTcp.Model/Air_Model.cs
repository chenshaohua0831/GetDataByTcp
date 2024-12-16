using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Model
{
    public class Air_Model
    {
        public int Id { get; set; }
        public string StationCode { get; set; }
        public DateTime TimePoint { get; set; }
        public string TimePointStr => TimePoint.ToString("yyyy-MM-dd HH:mm:ss");
        public decimal Value { get; set; }
        public DateTime OperationTime { get; set; }
        public string OperationTimeStr => OperationTime.ToString("yyyy-MM-dd HH:mm:ss");
        public string Mark { get; set; }
        public string Mark1 { get; set; }
        public string Mark2 { get; set; }

        //外联
        public string PositionName { get; set; }
        /// <summary>
        /// 0：正常 1：3小时恒值 2：负值 3：0值 4：空白值 
        /// </summary>
        public int DataStatus { get; set; } = 0;
        public string ValueStr { get; set; }
        public string Operator { get; set; }
        /// <summary>
        /// 是否人工修改过，0否1是
        /// </summary>
        public int IsUpate { get; set; } = 0;
    }
}
