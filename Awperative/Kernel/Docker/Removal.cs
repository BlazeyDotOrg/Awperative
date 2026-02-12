namespace Awperative;

public abstract partial class Docker
{
    public void Remove(Behavior behavior) {
        
        if(!_components.Contains(behavior)) { Debug.LogError("Body does not have a component of this type"); return; }
        
        behavior.End();
        _components.Remove(behavior);
    }
    
    public void Remove<Generic>() where Generic : Behavior {
        try
        {
            Behavior foundBehavior = Get<Generic>();

            foundBehavior.End();
            _components.Remove(foundBehavior);
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void RemoveAll<Generic>() where Generic : Behavior {
        try {
            foreach (Behavior component in GetAll<Generic>()) {
                component.End();
                _components.Remove(component);
            }
        }catch { Debug.LogError("Removal failed"); }
    }
}