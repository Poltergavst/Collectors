using System;
using UnityEngine;

public class SerializeInterfaceAttribute : PropertyAttribute
{
    public Type Type { get; private set; }

    public SerializeInterfaceAttribute(Type type)
    {
        Type = type;
    }
}
