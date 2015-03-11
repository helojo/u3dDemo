using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleMainTalkPop : MonoBehaviour
{
    private BattleData battleGameData;
    public bool immortal;
    private System.Action myOnFinish;
    private LabelTextHelper TalkTextlabel;

    private void OnClickTalkBox(GameObject obj)
    {
        if (!this.TalkTextlabel.GetIsFinish())
        {
            this.TalkTextlabel.FinishText();
        }
        else
        {
            base.StopAllCoroutines();
            this.OnShowTalkFinished();
        }
    }

    private void OnShowTalkFinished()
    {
        if (this.myOnFinish != null)
        {
            this.myOnFinish();
        }
    }

    public void ShowTalkPop(int pos, int cardEntry, string name, string text, System.Action OnFinish, BattleData _battleGameData)
    {
        base.transform.localScale = Vector3.one;
        this.TryInit();
        this.myOnFinish = OnFinish;
        this.battleGameData = _battleGameData;
        this.TalkTextlabel.text = text;
        int activeWidth = GUIMgr.Instance.Root.activeWidth;
        int activeHeight = GUIMgr.Instance.Root.activeHeight;
        base.transform.localPosition = new Vector3((float) Mathf.FloorToInt(-0.5f * activeWidth), (float) Mathf.FloorToInt(-0.5f * activeHeight), 0f);
        base.transform.FindChild("panel/bg").GetComponent<UIWidget>().width = activeWidth + 0x7d;
        base.transform.FindChild("panel/text").GetComponent<UIWidget>().width = activeWidth - 0x155;
    }

    [DebuggerHidden]
    private IEnumerator StartShow()
    {
        return new <StartShow>c__Iterator97 { <>f__this = this };
    }

    private void TryInit()
    {
        if (this.TalkTextlabel == null)
        {
            UIEventListener listener1 = UIEventListener.Get(base.gameObject);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickTalkBox));
            this.TalkTextlabel = base.gameObject.transform.FindChild("panel/text").gameObject.GetComponent<LabelTextHelper>();
        }
    }

    [CompilerGenerated]
    private sealed class <StartShow>c__Iterator97 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleMainTalkPop <>f__this;
        internal float <time>__0;

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
                    this.<time>__0 = BattleGlobal.TalkBoxShowTime;
                    if ((this.<>f__this.battleGameData != null) && this.<>f__this.battleGameData.isAuto)
                    {
                        this.<time>__0 = BattleGlobal.AutoCountDownTime;
                    }
                    this.$current = new WaitForSeconds(this.<time>__0);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.OnShowTalkFinished();
                    this.$PC = -1;
                    break;
            }
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
}

