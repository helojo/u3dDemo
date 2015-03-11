using System;

public class NGuiPlayMakerEvents
{
    public static string GetFsmEventEnumValue(Enum value)
    {
        string str = null;
        PlayMakerNGUI_FsmEvent[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(PlayMakerNGUI_FsmEvent), false) as PlayMakerNGUI_FsmEvent[];
        if ((customAttributes != null) && (customAttributes.Length > 0))
        {
            str = customAttributes[0].Value;
        }
        return str;
    }
}

