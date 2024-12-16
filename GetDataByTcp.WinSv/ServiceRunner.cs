using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace GetDataByTcp.WinSv
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly Task<IScheduler> scheduler;

        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public bool Start(HostControl hostControl)
        {
            scheduler.Result.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            scheduler.Result.Shutdown(true);
            return true;
        }

        public bool Continue(HostControl hostControl)
        {

            scheduler.Result.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            scheduler.Result.PauseAll();
            return true;
        }


    }
}
