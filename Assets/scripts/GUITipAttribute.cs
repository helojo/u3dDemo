using System;
using System.Runtime.CompilerServices;

public class GUITipAttribute : Attribute
{
    public GUITipAttribute(string resources)
    {
        this.ResourcesName = resources;
    }

    public string ResourcesName { get; set; }
}

