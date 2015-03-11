using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PushMgr : MonoBehaviour
{
    private bool isDirty;
    private float lastTipTime;
    public static PushMgr mInstance;
    private Dictionary<int, Notify> notifyMap = new Dictionary<int, Notify>();
    public Queue<string> tips = new Queue<string>();

    public void addNotification(int id, string actionStr, string bodyStr, int second)
    {
        if (SettingMgr.mInstance.PushNotification_Enable)
        {
            this.isDirty = true;
            Notify notify = new Notify {
                notifyid = id,
                action = actionStr,
                body = bodyStr,
                secondSinceNow = second,
                expiredTime = DateTime.Now.AddSeconds((double) second)
            };
            this.notifyMap[id] = notify;
        }
    }

    private void Awake()
    {
        mInstance = this;
    }

    private void checkAndTipDlg()
    {
        if ((this.tips.Count > 0) && ((Time.realtimeSinceStartup - this.lastTipTime) > 0.8f))
        {
            this.lastTipTime = Time.realtimeSinceStartup;
            TipsDiag.SetText(this.tips.Dequeue());
        }
    }

    private void clearIconBadgeNumber()
    {
    }

    private void FixedUpdate()
    {
        this.checkAndTipDlg();
        if (this.isDirty)
        {
            this.isDirty = false;
            this.removeAllNotifications();
            int iconNumber = 1;
            foreach (int num2 in this.notifyMap.Keys)
            {
                Notify notify = this.notifyMap[num2];
                if (notify.expiredTime > DateTime.Now)
                {
                    this.realcreateNotification(this.notifyMap[num2], iconNumber);
                    iconNumber++;
                }
            }
            if (iconNumber == 1)
            {
                this.clearIconBadgeNumber();
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            this.isDirty = true;
            this.OnIOSResume();
        }
        else
        {
            this.OnIOSPause();
        }
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    public void OnIOSPause()
    {
        if (SettingMgr.mInstance.PushNotification_Enable && ActorData.getInstance().IsInited())
        {
            if (SettingMgr.mInstance.PushTiLiFull_Enable)
            {
                int phyForceFullAllTime = ActorData.getInstance().GetPhyForceFullAllTime();
                if (phyForceFullAllTime > 0)
                {
                    this.PushIOSNotice(ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f61), phyForceFullAllTime);
                }
            }
            if (GameDataMgr.Instance.boostRecruit.valid)
            {
                long time = 0L;
                if ((SettingMgr.mInstance.FreeTakeCardPush && !GameDataMgr.Instance.boostRecruit.FreeTime(0, out time)) && (time > 0L))
                {
                    this.PushIOSNotice(ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f62), (int) time);
                }
            }
            if (SettingMgr.mInstance.ShopRefreshPush)
            {
                long nextShopRefreshTime = GameDataMgr.Instance.GetNextShopRefreshTime();
                if (nextShopRefreshTime > 0L)
                {
                    this.PushIOSNotice(ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f63), (int) nextShopRefreshTime);
                }
            }
            if (SettingMgr.mInstance.SkillFullPush)
            {
                int skillPointFullTime = ActorData.getInstance().GetSkillPointFullTime();
                if (skillPointFullTime > 0)
                {
                    this.PushIOSNotice(ConfigMgr.getInstance().GetWord(0xa95f60), ConfigMgr.getInstance().GetWord(0xa95f65), skillPointFullTime);
                }
            }
        }
    }

    private void OnIOSResume()
    {
    }

    private void PushIOSNotice(string actionStr, string bodyStr, int second)
    {
    }

    private void realcreateNotification(Notify noti, int iconNumber)
    {
        if (!Application.isEditor)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.U3dNotification");
            object[] args = new object[] { noti.notifyid, noti.action, noti.body, noti.secondSinceNow };
            class2.CallStatic("pushNotification", args);
        }
    }

    private void removeAllNotifications()
    {
        if (!Application.isEditor)
        {
            new AndroidJavaClass("com.locojoy.mtd.U3dNotification").CallStatic("clearNotifications", new object[0]);
        }
    }

    private void Start()
    {
        this.clearIconBadgeNumber();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Notify
    {
        public int notifyid;
        public string action;
        public string body;
        public int secondSinceNow;
        public DateTime expiredTime;
    }
}

