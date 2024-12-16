using Dapper;
using GetDataByTcp.BLL;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GetDataByTcp.WinSv.Jobs
{
    // 帖子自动分发
    [DisallowConcurrentExecution]
    public class AirPM25Job : IJob
    {
        static bool runing = false;

        public Task Execute(IJobExecutionContext context)
        {

            if (runing)
            {
                Console.WriteLine("running...");

                try
                {
                    var fireTime = context.FireTimeUtc.LocalDateTime;
                    doit(fireTime);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("running end...");
                runing = true;
                return Task.CompletedTask;
            }
            Console.WriteLine("calling...");
            runing = true;
            Console.WriteLine("calling end...");
            return Task.CompletedTask;

        }
        public void doit(DateTime fireTime)
        {
            Air_BLL bll = new Air_BLL();
            var dateStart = fireTime.AddMinutes(-26).ToString("yyyy-MM-dd HH:mm:ss");
            var dateEnd = fireTime.AddMinutes(-21).ToString("yyyy-MM-dd HH:mm:ss");
            bll.Insert_Air_5m_Job(dateStart, dateEnd);
        }
    }
}
