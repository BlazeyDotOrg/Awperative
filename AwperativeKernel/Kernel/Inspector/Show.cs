using System;


namespace AwperativeKernel;


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class Show : Attribute
{
    public bool UseInspectorDefaults = true;
}