using System;
using System.Collections.Generic;


public class m_taskinfo_uid
{
    public string task_queue_id { get; set; }

    public AccountTask taskdoc { get; set; }

    /// <summary>
    /// token
    /// </summary>
    public string token { get; set; }


}
public class AccountTask
{
    public object _id { get; set; }

    /// <summary>
    /// 标记 默认0 中控如果设置了这个值，则只按这个值读
    /// </summary>
    public int Tag { get; set; }

    public int AccountID { get; set; }

    public string AccountType { get; set; }

    public string AccountUser { get; set; }
    public string AccountPwd { get; set; }

    /// <summary>
    /// 任务列表
    /// </summary>
    public List<TaskInfo> Task { get; set; }

    /// <summary>
    /// 执行状态
    /// </summary>
    public int RState { get; set; }
    /// <summary>
    /// 添加日期
    /// </summary>
    public DateTime AddDate { get; set; }

    /// <summary>
    /// 锁定日期
    /// </summary>
    public DateTime LockDate { get; set; }




}

public class TaskInfo
{
    /// <summary>
    /// 任务起始地址
    /// </summary>
    public string URL { get; set; }
    /// <summary>
    /// 任务唯一标识
    /// </summary>
    public string ID { get; set; }
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 任务最大时长(秒)
    /// </summary>
    public int Timeout { get; set; }
    /// <summary>
    /// 角本数据
    /// </summary>
    public string JSData { get; set; }
    /// <summary>
    /// 角本地址
    /// </summary>
    public string JSLink { get; set; }
    /// <summary>
    /// 任务角本
    /// </summary>
    public string JSBody { get; set; }
    /// <summary>
    /// 任务状态
    /// </summary>
    public int State { get; set; }
    /// <summary>
    /// 任务添加时间
    /// </summary>
    public DateTime AddDate { get; set; }


    /// <summary>
    /// 发布时间（生效时间）
    /// </summary>
    public DateTime PubDate { get; set; }

    /// <summary>
    /// 任务完成时间
    /// </summary>
    public DateTime ComplateDate { get; set; }
    /// <summary>
    /// UA
    /// </summary>
    public string UserAgent { get; set; }
    /// <summary>
    /// 窗口分辨率
    /// </summary>
    public DPI Screen { get; set; }
    /// <summary>
    /// 代理
    /// </summary>
    public ProxyInfo Agent { get; set; }
    /// <summary>
    /// 图片禁用状态
    /// </summary>
    public bool DisableImage { get; set; }

    /// <summary>
    /// 优先级 越大优先级越高
    /// </summary>
    public int Level { get; set; }


    public class ProxyInfo
    {
        public string IP { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Pwd { get; set; }

    }
    public class DPI
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int ScaleFactor { get; set; }
    }
}