using FastBuf;
using System;

public class TencentStoreCommodity
{
    public bool canBuy;
    public int commodityId;
    public CostType costType = CostType.E_CT_Stone;
    public UniversialRewardDrawFlag flag;
    public TxActivityRewardDescribeConfig itemsData = new TxActivityRewardDescribeConfig();
    public int levelLimit;
    public string mainDescribe;
    public int purchaseCount;
    public int purchaseCountOfDay;
    public string reward_describe;
    public string reward_items = "99|wy_buff_icon3|1|潘多拉宝盒|1|这是一个神奇的宝盒。\n[078dff]( 我写什么数据，里面就会有什么宝贝。 )";
    public int reward_Price;
    public int serverPurCnt;
    public int serverPurCntOfDay;
}

