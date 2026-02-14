using System;
using System.Collections.Generic;

namespace Awperative;

public abstract partial class Docker
{
    
    
    internal HashSet<Behavior> _behaviors = [];
    
    public Behavior Add<Generic>() where Generic : Behavior => Add<Generic>([]);
    public Behavior Add<Generic>(object[] __args) where Generic : Behavior {
        if(typeof(Generic).GetConstructor((Type[]) __args) == null) { Debug.LogError("Component does not contain a valid constructor"); return null; };

        try {
            Behavior behavior = (Generic)Activator.CreateInstance(typeof(Generic), __args);
            
            if(behavior == null) { Debug.LogError("Failed to create component"); return null; }
            
            _behaviors.Add(behavior);
            behavior.Initiate(this);
            return behavior;
            
        }catch { Debug.LogError("Failed to create component"); return null; }
    }
    
    
    
    
    
    public Behavior Get<Generic>() where Generic : Behavior => GetAll<Generic>()[0];
    public Behavior[] GetAll<Generic>() where Generic : Behavior {
        
        List<Behavior> returnValue = [];
        foreach (Behavior component in _behaviors)
            if (component is Generic) returnValue.Add(component);
        
        if(returnValue.Count == 0) { Debug.LogWarning("Scene has no components of this type"); return null; }
        
        return returnValue.ToArray();
    }
    
    
    
    
    
    public void Destroy(Behavior behavior) {
        
        if(!_behaviors.Contains(behavior)) { Debug.LogError("Body does not have a component of this type"); return; }
        
        behavior.Destroy();
        _behaviors.Remove(behavior);
    }
    
    public void Destroy<Generic>() where Generic : Behavior {
        try
        {
            Behavior foundBehavior = Get<Generic>();

            foundBehavior.Destroy();
            _behaviors.Remove(foundBehavior);
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void DestroyAll<Generic>() where Generic : Behavior {
        try {
            foreach (Behavior component in GetAll<Generic>()) {
                component.Destroy();
                _behaviors.Remove(component);
            }
        }catch { Debug.LogError("Removal failed"); }
    }

    public void Remove(Behavior behavior)
    {
        if(!_behaviors.Contains(behavior)) { Debug.LogError("Body does not have a component of this type"); return; }
        
        _behaviors.Remove(behavior);
    }
    
    public void Remove<Generic>() where Generic : Behavior {
        try
        {
            Behavior foundBehavior = Get<Generic>();

            _behaviors.Remove(foundBehavior);
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void RemoveAll<Generic>() where Generic : Behavior {
        try {
            foreach (Behavior component in GetAll<Generic>()) {
                _behaviors.Remove(component);
            }
        }catch { Debug.LogError("Removal failed"); }
    }
}