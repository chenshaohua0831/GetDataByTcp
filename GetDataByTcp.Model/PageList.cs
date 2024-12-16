using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Model
{
    public class PageList<T>
    {
        public List<T> DataList { get; set; }
        public int Count { get; set; }
    }
}
