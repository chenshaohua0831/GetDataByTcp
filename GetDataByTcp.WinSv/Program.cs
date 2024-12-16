using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Management;
using Topshelf;

namespace GetDataByTcp.WinSv
{

    /// <summary>
    /// The server's main entry point.
    /// </summary>
    public static class Program
    {

        /// <summary>
        /// Main.
        /// </summary>
        public static void Main()
        {
            //代码加这里

            #region init


            // change from service account's dir to more logical one
            Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();

                x.Service<ServiceRunner>();
                string svname = "videopub" + DateTime.Now.Second;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("svname"))
                {
                    svname = ConfigurationManager.AppSettings["svname"];

                }

                x.SetDescription(svname);
                x.SetDisplayName(svname);
                x.SetServiceName(svname);

                x.EnablePauseAndContinue();
            });


            #endregion
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            

        }

    }
}
