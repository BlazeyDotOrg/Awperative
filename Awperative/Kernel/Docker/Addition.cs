using System;

namespace Awperative;

public abstract partial class Docker 
{
    
    public Behavior Add<Generic>() where Generic : Behavior => Add<Generic>([]);
    public Behavior Add<Generic>(object[] __args) where Generic : Behavior {    
        if(typeof(Generic).GetConstructor((Type[]) __args) == null) { Debug.LogError("Component does not contain a valid constructor"); return null; };

        try {
            Behavior behavior = (Generic)Activator.CreateInstance(typeof(Generic), __args);
            
            if(behavior == null) { Debug.LogError("Failed to create component"); return null; }
            
            _components.Add(behavior);
            behavior.Initiate(this);
            return behavior;
            
        }catch { Debug.LogError("Failed to create component"); return null; }
    }
}