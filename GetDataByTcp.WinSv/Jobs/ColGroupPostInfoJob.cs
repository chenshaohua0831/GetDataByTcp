using Dapper;
using Newtonsoft.Json;
using Quartz;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WuWei.Auto.Collection.Models;
using WuWei.Auto.Collection.Repo;
using WuWei.Auto.Collection.Utils;

namespace WuWei.Auto.Collect.WinSv.Jobs
{
    [DisallowConcurrentExecution]
    public class ColGroupPostInfoJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("是否存在要检测的专页" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var sql = $"SELECT * FROM `distribute_auto_record` where  distribute_publish_time = last_collect_time and status=2 and record_type=2 and last_collect_time > DATE_ADD(NOW(), INTERVAL 25 HOUR);  ";
                var alist = WuWeiDBFactory.Execute((conn) => conn.Query<DistributeAutoRecordModel>(sql, null)).AsList();
                Console.WriteLine(alist.Count);

                foreach(var item in alist)
                {

                    var info = SmMethodsHelper.getUserInfo(item.post_email);
                    dynamic d = JsonConvert.DeserializeObject<dynamic>(info);
                    if (info == "[]" )
                    {
                        
                        Console.WriteLine("没查到" + item.post_email);

                    }
                    else
                    {
                        var fbid = MatchID(d[0].channel);
                        var url = "https://m.facebook.com/profile.php?id="+ fbid + "&groupid="+item.page_group_id;
                        string data = $"var id={item.id};var url='{url}';var msg='{item.distribute_title}';";
                        var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        WuWeiDBFactory.Execute((conn) => conn.Execute($"update distribute_auto_record set last_collect_time='{time}'", new { }));
                        JobHelper.CreateJob(367, 86, d[0].id, data, "http://admin.okaymw.com/taskJs/CheckFBReels.js", "采自动社团贴数据:" + item.post_email, DateTime.Now, 600, "https://www.facebook.com");
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + item.page_group_name + "生成检测任务成功");
                    }

                   
                }


                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string MatchID(string channel)
        {
            // 定义正则表达式
            string pattern = @"id=(\d+)";

            // 创建 Regex 对象并进行匹配
            Regex regex = new Regex(pattern);
            Match match = regex.Match(channel);

            // 提取匹配到的 id
            string id = match.Groups[1].Value;

            return id;
        }
    }

}

