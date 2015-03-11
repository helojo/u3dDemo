using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class AdManager : XSingleton<AdManager>
{
    public bool _isAd;
    [CompilerGenerated]
    private static Comparison<ServerInfo.GameAdInfo> <>f__am$cache7;
    public List<AdTexture> AdPics = new List<AdTexture>();
    private int count;
    private int finishCount;
    public System.Action OnCompleted;
    public string strAdTime;
    public List<string> tempUrls = new List<string>();

    private void ActionCompleted()
    {
        if (this.finishCount == this.count)
        {
            Debug.Log("adpic OnCompleted");
            if (this.OnCompleted != null)
            {
                this.OnCompleted();
            }
        }
    }

    public void BeginDown(List<ServerInfo.GameAdInfo> urls)
    {
        this.AdPics.Clear();
        this.finishCount = 0;
        this.count = urls.Count;
        string str = BundleMgr.PathOfArchive();
        foreach (ServerInfo.GameAdInfo info in urls)
        {
            if (!string.IsNullOrEmpty(info.Url))
            {
                int num = info.Url.LastIndexOf("/", StringComparison.Ordinal);
                if (num != -1)
                {
                    string pictureName = info.Url.Substring(num + 1);
                    string path = str + "/" + pictureName;
                    if (File.Exists(path))
                    {
                        ScheduleMgr.Instance.RunWork(this.Loader("file://" + path, false, pictureName, info.Desc, info.ShowType));
                    }
                    else
                    {
                        ScheduleMgr.Instance.RunWork(this.Loader(info.Url, true, pictureName, info.Desc, info.ShowType));
                    }
                }
                else
                {
                    ScheduleMgr.Instance.RunWork(this.Loader(info.Url, true, "adtemp.png", info.Desc, info.ShowType));
                }
            }
            else
            {
                this.finishCount++;
                Debug.Log("ad uri 空");
                this.ActionCompleted();
            }
        }
    }

    public void CallBack(Texture text, string desc, int showType, string url, string strPicName)
    {
        if (text != null)
        {
            AdTexture item = new AdTexture {
                desc = desc,
                texture = text,
                ShowType = showType,
                strUrl = url,
                strPictureName = strPicName
            };
            this.AdPics.Add(item);
        }
    }

    [DebuggerHidden]
    private IEnumerator Loader(string url, bool isNew, string pictureName, string desc, int showType)
    {
        return new <Loader>c__IteratorA8 { url = url, desc = desc, showType = showType, pictureName = pictureName, isNew = isNew, <$>url = url, <$>desc = desc, <$>showType = showType, <$>pictureName = pictureName, <$>isNew = isNew, <>f__this = this };
    }

    public void SaveAd(string adTime, List<string> adUrlsList)
    {
        if ((adTime == this.strAdTime) && (this.tempUrls.Count > 0))
        {
            this.tempUrls.ForEach(new Action<string>(adUrlsList.Add));
        }
        if (adUrlsList.Count > 0)
        {
            <SaveAd>c__AnonStorey286 storey = new <SaveAd>c__AnonStorey286 {
                dataSave = string.Empty
            };
            adUrlsList.ForEach(new Action<string>(storey.<>m__5D6));
            SettingMgr.mInstance.SetCommonString(GameDefine.getInstance().lastAccountName.ToUpper(), adTime.ToUpper(), storey.dataSave);
        }
    }

    public List<ServerInfo.GameAdInfo> SeletAdInfos()
    {
        List<ServerInfo.GameAdInfo> list = new List<ServerInfo.GameAdInfo>();
        this.tempUrls.Clear();
        this.strAdTime = TimeMgr.Instance.ServerDateTime.ToString("yyyy-MM-dd") + GameDefine.getInstance().lastAccountName + ServerInfo.lastGameServerId.ToString();
        string str = SettingMgr.mInstance.GetCommonString(GameDefine.getInstance().lastAccountName.ToUpper(), this.strAdTime.ToUpper(), string.Empty);
        if (!string.IsNullOrEmpty(str))
        {
            Debug.Log(str);
            this.tempUrls = StrParser.ParseStringList(str, "|");
        }
        List<ServerInfo.GameAdInfo> gameAdInfos = ServerInfo.getInstance().gameAdInfos;
        if ((gameAdInfos != null) && (gameAdInfos.Count > 0))
        {
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = (a1, a2) => a2.Priority - a1.Priority;
            }
            gameAdInfos.Sort(<>f__am$cache7);
            foreach (ServerInfo.GameAdInfo info in gameAdInfos)
            {
                List<string> list3 = StrParser.ParseStringList(info.GameServerId, ",");
                if ((info.GameServerId == "-1") || list3.Contains(ServerInfo.lastGameServerId.ToString()))
                {
                    List<string> list4 = StrParser.ParseStringList(info.UserType, ",");
                    int tencentType = (int) GameDefine.getInstance().GetTencentType();
                    if (list4.Contains(tencentType.ToString()))
                    {
                        if (info.ShowType == 0)
                        {
                            string item = string.Empty;
                            if (string.IsNullOrEmpty(info.Url))
                            {
                                continue;
                            }
                            int num2 = info.Url.LastIndexOf("/", StringComparison.Ordinal);
                            if (num2 != -1)
                            {
                                item = info.Url.Substring(num2 + 1);
                            }
                            if (this.tempUrls.Contains(item))
                            {
                                continue;
                            }
                        }
                        if ((DateTime.Compare(TimeMgr.Instance.ServerDateTime, info.StartTime) >= 0) && (DateTime.Compare(TimeMgr.Instance.ServerDateTime, info.EndTime) < 0))
                        {
                            list.Add(info);
                        }
                        else
                        {
                            DateTime time = info.StartTime.AddSeconds((double) info.Timing);
                            DateTime time2 = info.EndTime.AddSeconds((double) info.Timing);
                            if ((DateTime.Compare(TimeMgr.Instance.ServerDateTime, time) >= 0) && (DateTime.Compare(TimeMgr.Instance.ServerDateTime, time2) < 0))
                            {
                                list.Add(info);
                            }
                        }
                    }
                }
            }
        }
        return list;
    }

    [CompilerGenerated]
    private sealed class <Loader>c__IteratorA8 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>desc;
        internal bool <$>isNew;
        internal string <$>pictureName;
        internal int <$>showType;
        internal string <$>url;
        internal AdManager <>f__this;
        internal byte[] <dataBytes>__2;
        internal float <time>__1;
        internal WWW <www>__0;
        internal string desc;
        internal bool isNew;
        internal string pictureName;
        internal int showType;
        internal string url;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<www>__0 = new WWW(this.url);
                    this.<time>__1 = 3f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0200;
            }
            if (!this.<www>__0.isDone && (this.<time>__1 > 0f))
            {
                this.<time>__1 -= Time.deltaTime;
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            this.<>f__this.finishCount++;
            if (!string.IsNullOrEmpty(this.<www>__0.error))
            {
                Debug.Log("ad uri error");
                this.<>f__this.ActionCompleted();
            }
            else
            {
                if (this.<www>__0.isDone && (this.<www>__0.texture != null))
                {
                    Debug.Log("www is Done");
                    this.<>f__this.CallBack(this.<www>__0.texture, this.desc, this.showType, this.url, this.pictureName);
                    if (this.isNew)
                    {
                        Debug.Log("New");
                        if (!string.IsNullOrEmpty(this.pictureName))
                        {
                            Debug.Log("new   " + this.pictureName);
                            if (this.<www>__0.bytes != null)
                            {
                                Debug.Log("bytes is not  null");
                                this.<dataBytes>__2 = this.<www>__0.bytes;
                                if (this.<dataBytes>__2 != null)
                                {
                                    Debug.Log("dataBytes is not  null");
                                    Debug.Log("path==" + BundleMgr.PathOfArchive() + "/" + this.pictureName);
                                    File.WriteAllBytes(BundleMgr.PathOfArchive() + "/" + this.pictureName, this.<dataBytes>__2);
                                }
                            }
                        }
                    }
                }
                Debug.Log("顺序执行结束");
                this.<>f__this.ActionCompleted();
                this.$PC = -1;
            }
        Label_0200:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SaveAd>c__AnonStorey286
    {
        internal string dataSave;

        internal void <>m__5D6(string obj)
        {
            this.dataSave = this.dataSave + obj.ToString();
            this.dataSave = this.dataSave + "|";
        }
    }
}

