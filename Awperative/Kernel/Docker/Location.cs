using System;
using System.Collections.Generic;

namespace Awperative;

public abstract partial class Docker
{
    public Behavior Get<Generic>() where Generic : Behavior => GetAll<Generic>()[0];
    public Behavior[] GetAll<Generic>() where Generic : Behavior {
        
        List<Behavior> returnValue = [];
        foreach (Behavior component in _components)
            if (component is Generic) returnValue.Add(component);
        
        if(returnValue.Count == 0) { Debug.LogWarning("Scene has no components of this type"); return null; }
        
        return returnValue.ToArray();
    }
}