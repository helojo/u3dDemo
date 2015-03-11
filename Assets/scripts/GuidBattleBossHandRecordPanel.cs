using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleBossHandRecordPanel : GUIPanelEntity
{
    private bool bForTest = true;
    protected UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>> TableGridHandTable = new UITableManager<UIAutoGenItem<GridHandTableItemTemplate, GridHandTableItemModel>>();
    public List<HandRecordItemInfo> xxTestHoItemInfoList = new List<HandRecordItemInfo>();

    private void ClickDetail(int itemId)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.SetHandOutItemInfo(this.xxTestHoItemInfoList);
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridHandTable = base.FindChild<UIGrid>("GridHandTable");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.LabelHandRecordTitle = base.FindChild<UILabel>("LabelHandRecordTitle");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridHandTable.InitFromGrid(this.GridHandTable);
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridHandTable.Count = itemCnt;
    }

    public void ReceiveGuildDupDistributeHistory(S2C_GuildDupDistributeHistory dupDistributeHistoryData, bool bForTest = false)
    {
        this.xxTestHoItemInfoList.Clear();
        if (!bForTest)
        {
            List<HandRecordItemInfo> list = new List<HandRecordItemInfo>();
            int num = 0;
            foreach (GuildDupDistributeHistroy histroy in dupDistributeHistoryData.historyList)
            {
                num++;
                HandRecordItemInfo item = new HandRecordItemInfo {
                    entry = histroy.itemId
                };
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(histroy.itemId);
                if (_config != null)
                {
                    item.quality = _config.quality;
                }
                else
                {
                    item.quality = 0;
                    Debug.LogWarning("Cur Item-Config is Error!  :::" + histroy.itemId);
                }
                item.isAuto = histroy.isAuto;
                item.playerName = histroy.targetName;
                item.timestamp = histroy.timestamp;
                this.xxTestHoItemInfoList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                HandRecordItemInfo info2 = new HandRecordItemInfo {
                    entry = i,
                    quality = i,
                    isAuto = false,
                    playerName = ConfigMgr.getInstance().GetWord(0x186bd) + i
                };
                this.xxTestHoItemInfoList.Add(info2);
            }
        }
        this.SetHandOutItemInfo(this.xxTestHoItemInfoList);
    }

    public void SetHandOutItemInfo(List<HandRecordItemInfo> hoItemInfoList)
    {
        base.Depth = 250;
        this.InitUiGridData(hoItemInfoList.Count);
        int count = hoItemInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(hoItemInfoList[i].entry);
            if (_config != null)
            {
                this.TableGridHandTable[i].Model.Template.TextureItemIcon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            }
            else
            {
                Debug.LogWarning("item_config dont have this CardEntry!!___" + hoItemInfoList[i].entry);
            }
            this.TableGridHandTable[i].Model.Template.LabelPlayerName.text = hoItemInfoList[i].playerName;
            CommonFunc.SetEquipQualityBorder(this.TableGridHandTable[i].Model.Template.SpriteItemQulity, hoItemInfoList[i].quality, false);
            DateTime time = TimeMgr.Instance.ConvertToDateTime((long) hoItemInfoList[i].timestamp);
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            if (time.Hour < 10)
            {
                str3 = "0" + time.Hour;
            }
            else
            {
                str3 = time.Hour.ToString();
            }
            if (time.Minute < 10)
            {
                str4 = "0" + time.Minute;
            }
            else
            {
                str4 = time.Minute.ToString();
            }
            object[] objArray1 = new object[] { time.Month, ConfigMgr.getInstance().GetWord(0x57d), time.Day, ConfigMgr.getInstance().GetWord(0x57e), str3, ":", str4, ConfigMgr.getInstance().GetWord(0x186be) };
            str2 = string.Concat(objArray1);
            this.TableGridHandTable[i].Model.Template.LabelGetTime.text = str2;
            if (hoItemInfoList[i].isAuto)
            {
                string word = ConfigMgr.getInstance().GetWord(0x186bb);
                this.TableGridHandTable[i].Model.Template.LabelHandOutType.text = word;
            }
            else
            {
                string str6 = ConfigMgr.getInstance().GetWord(0x186bc);
                this.TableGridHandTable[i].Model.Template.LabelHandOutType.text = str6;
            }
        }
    }

    protected UIWidget AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UIGrid GridHandTable { get; set; }

    protected UILabel LabelHandRecordTitle { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    public class GridHandTableItemModel : TableItemModel<GuidBattleBossHandRecordPanel.GridHandTableItemTemplate>
    {
        public override void Init(GuidBattleBossHandRecordPanel.GridHandTableItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class GridHandTableItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ReqBossItem = base.FindChild<UIDragScrollView>("ReqBossItem");
            this.SpriteItemQulity = base.FindChild<UISprite>("SpriteItemQulity");
            this.TextureItemIcon = base.FindChild<UITexture>("TextureItemIcon");
            this.LabelPlayerName = base.FindChild<UILabel>("LabelPlayerName");
            this.LabelHandOutType = base.FindChild<UILabel>("LabelHandOutType");
            this.LabelGetTime = base.FindChild<UILabel>("LabelGetTime");
        }

        public UILabel LabelGetTime { get; private set; }

        public UILabel LabelHandOutType { get; private set; }

        public UILabel LabelPlayerName { get; private set; }

        public UIDragScrollView ReqBossItem { get; private set; }

        public UISprite SpriteItemQulity { get; private set; }

        public UITexture TextureItemIcon { get; private set; }
    }

    public class HandRecordItemInfo
    {
        public int entry;
        public bool isAuto;
        public string playerName;
        public int quality;
        public int timestamp;
    }
}

