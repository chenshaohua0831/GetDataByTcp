using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.DAL
{
    public class TestDAL
    {
        public int TestInsert()
        {
            using (var db = SQLHelper.OpenConnection())
            {
                string sql = "insert into test () values ()";
                return db.Execute(sql);
            }
        }
    }
}
