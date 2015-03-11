using System;

public class FsmEventUnitility
{
    public static string Enum2FsmEventName(Enum value)
    {
        string str = null;
        GUIFsmEventAttribute[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(GUIFsmEventAttribute), false) as GUIFsmEventAttribute[];
        if ((customAttributes != null) && (customAttributes.Length > 0))
        {
            str = customAttributes[0].Value;
        }
        return str;
    }
}

