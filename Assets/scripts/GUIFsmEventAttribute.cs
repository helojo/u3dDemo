using System;

public class GUIFsmEventAttribute : Attribute
{
    private string value;

    public GUIFsmEventAttribute(string val)
    {
        this.value = val;
    }

    public string Value
    {
        get
        {
            return this.value;
        }
    }
}

