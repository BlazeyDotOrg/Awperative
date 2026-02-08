using System;
using System.Collections.Generic;

namespace Awperative;

public sealed partial class Body
{
    public Component GetComponent<Generic>() where Generic : Component => GetComponents<Generic>()[0];
    public Component[] GetComponents<Generic>() where Generic : Component {
        
        List<Component> returnValue = [];
        foreach (Component component in _components)
            if (component is Generic) returnValue.Add(component);
        
        if(returnValue.Count == 0) { Debug.LogWarning("Scene has no components of this type"); return null; }
        
        return returnValue.ToArray();
    }
    
    public Generic FindSingleton<Generic>() where Generic : Component
    {
        foreach (Component component in _components)
            if (component.GetType() == typeof(Generic))
                if(component.EnforceSingleton)
                    return (Generic) component;
                else
                    throw new Exception("Component is not a singleton");
        
        throw new Exception("Component not found");
        return null;
    }
    
    public bool SingletonExists<Generic>() where Generic : Component
    {
        foreach (Component __component in _components)
            if (__component.GetType() == typeof(Generic))
                if (__component.EnforceSingleton)
                    return true;
                else
                    return false;
        
        return false;
    }
}