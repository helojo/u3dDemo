using System;

public class PlayMakerNGUI_FsmEvent : Attribute
{
    private readonly string _value;

    public PlayMakerNGUI_FsmEvent(string value)
    {
        this._value = value;
    }

    public string Value
    {
        get
        {
            return this._value;
        }
    }
}

