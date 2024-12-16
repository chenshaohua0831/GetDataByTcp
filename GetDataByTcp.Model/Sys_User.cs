using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.Model
{
    public class Sys_User
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string PWD { get; set; }
        public string UserName { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int Enabled { get; set; }
        public string Remark { get; set; }
    }
    public class Sys_User_Query
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Enabled { get; set; }
        public int PageIndex { get; set; }
        public int PageSize{ get; set; }

    }
}
