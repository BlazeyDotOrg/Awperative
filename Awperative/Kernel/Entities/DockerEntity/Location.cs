using System;
using System.Collections.Generic;

namespace Awperative;

public abstract partial class DockerEntity
{
    public Component GetComponent<Generic>() where Generic : Component => GetComponents<Generic>()[0];
    public Component[] GetComponents<Generic>() where Generic : Component {
        
        List<Component> returnValue = [];
        foreach (Component component in _components)
            if (component is Generic) returnValue.Add(component);
        
        if(returnValue.Count == 0) { Debug.LogWarning("Scene has no components of this type"); return null; }
        
        return returnValue.ToArray();
    }
}