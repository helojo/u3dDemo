using System;
using System.Runtime.CompilerServices;
using Toolbox;

public class GoldTreeConfrim : GUIEntity
{
    private System.Action Cancel;
    public UIButton CancelBtn;
    private UILabel lb_cost;
    private UILabel lb_receive;
    private System.Action Ok;
    public UIButton OkBtn;

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.CancelBtn = base.transform.FindChild<UIButton>("CancelBtn");
        this.OkBtn = base.transform.FindChild<UIButton>("OkBtn");
        this.lb_cost = base.transform.FindChild<UILabel>("lb_cost");
        this.lb_receive = base.transform.FindChild<UILabel>("lb_receive");
        this.CancelBtn.OnUIMouseClick(delegate (object user) {
            GUIMgr.Instance.ExitModelGUI(this);
            if (this.Cancel != null)
            {
                this.Cancel();
            }
        });
        this.OkBtn.OnUIMouseClick(delegate (object user) {
            GUIMgr.Instance.ExitModelGUI(this);
            if (this.Ok != null)
            {
                this.Ok();
            }
        });
    }

    public void SetMessage(string cost, string receive)
    {
        this.lb_cost.text = cost;
        this.lb_receive.text = receive;
    }

    public static void Show(string cost, string receive, System.Action ok, System.Action cancel)
    {
        <Show>c__AnonStorey1FC storeyfc = new <Show>c__AnonStorey1FC {
            ok = ok,
            cancel = cancel,
            cost = cost,
            receive = receive
        };
        GUIMgr.Instance.DoModelGUI("GoldTreeConfrim", new Action<GUIEntity>(storeyfc.<>m__340), null);
    }

    [CompilerGenerated]
    private sealed class <Show>c__AnonStorey1FC
    {
        internal System.Action cancel;
        internal string cost;
        internal System.Action ok;
        internal string receive;

        internal void <>m__340(GUIEntity o)
        {
            GoldTreeConfrim confrim = o as GoldTreeConfrim;
            confrim.Ok = this.ok;
            confrim.Cancel = this.cancel;
            confrim.SetMessage(this.cost, this.receive);
        }
    }
}

