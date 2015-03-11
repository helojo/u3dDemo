using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

internal class GuildBattleNewRulePanel : GUIPanelEntity
{
    public RuleType curRuleType;
    private List<RuleItemInfo> guidBattleRuleList = new List<RuleItemInfo>();
    protected UITableManager<UIAutoGenItem<GridRuleItemTemplate, GridRuleItemModel>> TableGridRule = new UITableManager<UIAutoGenItem<GridRuleItemTemplate, GridRuleItemModel>>();

    public void GetRuleInfo()
    {
        this.guidBattleRuleList.Clear();
        for (int i = 0; i < 0x12; i++)
        {
            guilddup_des_config _config = ConfigMgr.getInstance().getByEntry<guilddup_des_config>(i);
            if (_config != null)
            {
                RuleItemInfo item = new RuleItemInfo {
                    ruleId = i,
                    ruleStr = _config.description
                };
                this.guidBattleRuleList.Add(item);
            }
        }
        this.InitGridInfo();
    }

    public void GetRuleInfo_DetainsDart()
    {
        this.guidBattleRuleList.Clear();
        for (int i = 0; i < 0x12; i++)
        {
            convoy_rule_config _config = ConfigMgr.getInstance().getByEntry<convoy_rule_config>(i);
            if (_config != null)
            {
                RuleItemInfo item = new RuleItemInfo {
                    ruleId = i,
                    ruleStr = _config.description
                };
                this.guidBattleRuleList.Add(item);
            }
        }
        this.InitGridInfo();
    }

    private void InitGridInfo()
    {
        this.TableGridRule.Cache = false;
        this.TableGridRule.Count = this.guidBattleRuleList.Count;
        int num = 0;
        for (int i = 0; i < this.TableGridRule.Count; i++)
        {
            this.TableGridRule[i].Model.Template.LabelRuleInfo.text = this.guidBattleRuleList[i].ruleStr;
            this.TableGridRule[i].Model.Template.LabelRuleInfo.MakePixelPerfect();
            Vector3 localPosition = this.TableGridRule[i].Model.Item.Root.transform.localPosition;
            this.TableGridRule[i].Model.Item.Root.transform.localPosition = new Vector3(localPosition.x, (float) -num, 0f);
            num += this.TableGridRule[i].Model.Template.LabelRuleInfo.height + 20;
            BoxCollider component = this.TableGridRule[i].Model.Item.Root.gameObject.GetComponent<BoxCollider>();
            if (component != null)
            {
                component.size = (Vector3) new Vector2(614f, (float) (this.TableGridRule[i].Model.Template.LabelRuleInfo.height + 20));
            }
            this.TableGridRule[i].Model.Template.SpritRuleBg.transform.localPosition = new Vector3(this.TableGridRule[i].Model.Item.Root.transform.localPosition.x, 20f, this.TableGridRule[i].Model.Item.Root.transform.localPosition.z);
            this.TableGridRule[i].Model.Template.SpritRuleBg.SetDimensions(0x274, this.TableGridRule[i].Model.Template.LabelRuleInfo.height + 5);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        RuleType curRuleType = this.curRuleType;
        if (curRuleType == RuleType.GuildBattle)
        {
            this.GetRuleInfo();
        }
        else if (curRuleType == RuleType.DetainsDart)
        {
            this.GetRuleInfo_DetainsDart();
        }
        else
        {
            this.GetRuleInfo();
        }
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridRule = base.FindChild<UIGrid>("GridRule");
        this.TableGridRule.InitFromGrid(this.GridRule);
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UIGrid GridRule { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    public class GridRuleItemModel : TableItemModel<GuildBattleNewRulePanel.GridRuleItemTemplate>
    {
        public override void Init(GuildBattleNewRulePanel.GridRuleItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }

        public int gridId { get; set; }
    }

    public class GridRuleItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ItemRule = base.FindChild<Transform>("ItemRule");
            this.SpritRuleBg = base.FindChild<UISprite>("SpritRuleBg");
            this.LabelRuleInfo = base.FindChild<UILabel>("LabelRuleInfo");
        }

        public Transform ItemRule { get; private set; }

        public UILabel LabelRuleInfo { get; private set; }

        public UISprite SpritRuleBg { get; private set; }
    }

    private class RuleItemInfo
    {
        public int ruleId;
        public string ruleStr;
    }

    public enum RuleType
    {
        GuildBattle,
        DetainsDart
    }
}

