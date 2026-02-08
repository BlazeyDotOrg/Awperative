namespace Awperative;

public sealed partial class Body
{
    public void RemoveComponent(Component __component) {
        
        if(!_components.Contains(__component)) { Debug.LogError("Body does not have a component of this type"); return; }
        
        __component.End();
        _components.Remove(__component);
    }
    
    public void RemoveComponent<Generic>() where Generic : Component {
        try
        {
            Component foundComponent = GetComponent<Generic>();

            foundComponent.End();
            _components.Remove(foundComponent);
            ComponentDestroyedEvent?.Invoke(this, new ComponentDestroyEvent(foundComponent, this, Scene));
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void RemoveComponents<Generic>() where Generic : Component {
        try {
            foreach (Component component in GetComponents<Generic>()) {
                component.End();
                _components.Remove(component);
                ComponentDestroyedEvent?.Invoke(this, new ComponentDestroyEvent(component, this, Scene));
            }
        }catch { Debug.LogError("Removal failed"); }
    }
}