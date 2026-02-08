using System;

namespace Awperative;

public sealed partial class Body 
{
    
    public Component AddComponent<Generic>() where Generic : Component => AddComponent<Generic>([]);
    public Component AddComponent<Generic>(object[] __args) where Generic : Component {
        
        if (SingletonExists<Generic>()) { Debug.LogError("Cannot add component when singleton exists"); return null; }
        if(typeof(Generic).GetConstructor((Type[]) __args) == null) { Debug.LogError("Component does not contain a valid constructor"); return null; };

        try {
            Component component = (Generic)Activator.CreateInstance(typeof(Generic), __args);
            
            if(component == null) { Debug.LogError("Failed to create component"); return null; }
            
            _components.Add(component);
            component.Initiate(this);
            return component;
            
        }catch { Debug.LogError("Failed to create component"); return null; }
    }
}