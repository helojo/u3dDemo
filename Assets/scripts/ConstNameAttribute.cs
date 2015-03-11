using System;
using System.Runtime.CompilerServices;

public class ConstNameAttribute : Attribute
{
    public ConstNameAttribute(string name) : this(name, -1)
    {
    }

    public ConstNameAttribute(string name, int defaultValue)
    {
        this.Name = name;
        this.DefaultValue = defaultValue;
    }

    public int DefaultValue { get; set; }

    public string Name { get; set; }
}

