using HutongGames.PlayMaker;
using System;

[ActionCategory(ActionCategory.String), Tooltip("Compares a string value to multiple options")]
public class StringCompareMulti : FsmStateAction
{
    [Tooltip("When true, compare strings on case")]
    public FsmBool CaseSensitive;
    [Tooltip("Event to raise on match")]
    public FsmEvent[] CompareEvents;
    [UIHint(UIHint.Variable), CompoundArray("Strings", "CompareTo", "CompareEvent"), Tooltip("String to compare to")]
    public FsmString[] CompareTos;
    [Tooltip("When true, runs on every frame")]
    public bool everyFrame;
    [Tooltip("Event to raise if no matches are found")]
    public FsmEvent NoMatchEvent;
    [RequiredField, Tooltip("String to check")]
    public FsmString StringVariable;

    private void DoStringCompare()
    {
        if (((this.StringVariable != null) && (this.CompareTos != null)) && (this.CompareEvents != null))
        {
            bool flag = (this.CaseSensitive != null) && this.CaseSensitive.Value;
            string str = !flag ? this.StringVariable.Value.ToLower() : this.StringVariable.Value;
            int length = this.CompareTos.Length;
            for (int i = 0; i < length; i++)
            {
                string str2 = !flag ? this.CompareTos[i].Value.ToLower() : this.CompareTos[i].Value;
                if (str.Equals(str2, StringComparison.CurrentCulture))
                {
                    base.Fsm.Event(this.CompareEvents[i]);
                    return;
                }
            }
            if (this.NoMatchEvent != null)
            {
                base.Fsm.Event(this.NoMatchEvent);
            }
        }
    }

    public override void OnEnter()
    {
        this.DoStringCompare();
        if (!this.everyFrame)
        {
            base.Finish();
        }
    }

    public override void OnUpdate()
    {
        this.DoStringCompare();
    }

    public override void Reset()
    {
        this.StringVariable = null;
        this.CompareTos = null;
        this.CompareEvents = null;
        this.NoMatchEvent = null;
        this.everyFrame = false;
    }
}

