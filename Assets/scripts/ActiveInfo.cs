using FastBuf;
using System;
using System.Collections.Generic;

public class ActiveInfo
{
    public string activity_describe = string.Empty;
    public string activity_name = string.Empty;
    public string activity_PicName = "Ui_Active_Bg_scgg";
    public string activity_PicNameDetail = string.Empty;
    public bool activity_showUsePic;
    public string activity_time_describe;
    public ActivityType activity_type;
    public int activity_unid;
    public int ActParameter;
    public int cdTime;
    public int dayParameter;
    public int entry;
    public UniversialRewardDrawFlag flag;
    public int holdOnTime;
    public bool is_new;
    public uint realStartTime;
    public List<TxActivityRewardDescribeConfig> rewards_configs = new List<TxActivityRewardDescribeConfig>();
    public ActiveShowType showType;
    public int sort_priority;
    public string start_time = string.Empty;
    public uint startTime;
    public List<TencentStoreCommodity> storeList = new List<TencentStoreCommodity>();
}

