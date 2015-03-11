using FastBuf;
using System;
using System.Collections.Generic;

public class TxActivityRewardDescribeConfig
{
    public int cd_time;
    public int duration;
    public int entry;
    public List<TxActivityCollectConfig> exchangeNeedConfig;
    public UniversialRewardDrawFlag flag = UniversialRewardDrawFlag.E_UNIREWARD_FLAG_HAVE_DRAW;
    public string reward_describe;
    public string reward_items = "99|icon_B6_kbzw|20|熊大无二|1|这是一个完全不存在的道具。\n[078dff]( 你想填什么就是什么~ )#1|112|2#0|25|1#3|740|10#2|1014|8#1|12|1";
    public List<SingleUniversialRewardConfig> rewardConfig = new List<SingleUniversialRewardConfig>();
    public int start_time;
    public bool subTimeEnable;
    public int type;
}

