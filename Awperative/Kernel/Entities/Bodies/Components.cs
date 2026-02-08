using System;
using System.Collections.Generic;


namespace Awperative;

public sealed partial class Body
{
    
    public Generic AddComponent<Generic>(object[] args) where Generic : Component {
        if (SingletonExists<Generic>()) { Debug.LogError("Cannot add component when singleton exists!"); return null; }

        Generic component = (Generic) Activator.CreateInstance(typeof(Generic), args);

        if (component == null) { Debug.LogError("Failed to create component!"); return null; }

        _components.Add(component);
        component.Initiate(this);
        
        ComponentCreatedEvent?.Invoke(this, new ComponentCreateEvent(component, this, Scene));
        
        return component;
    }
    
    public Generic AddComponent<Generic>() where Generic : Component {

        if (SingletonExists<Generic>())
            throw new Exception("Cannot add component when singleton exists!");
        
        Generic component = (Generic) Activator.CreateInstance(typeof(Generic));
        
        if(component == null)
            throw new Exception("Failed to create component!");
        
        components.Add(component);
        component.Initiate(this);
        
        ComponentCreatedEvent?.Invoke(this, new ComponentCreateEvent(component, this, scene));
        
        return component;
    }
    
    public Generic[] GetComponents<Generic>() where Generic : Component {
        
        List<Component> foundComponents = components.FindAll(x => x.GetType() == typeof(Generic));
        
        if(foundComponents.Count == 0)
            throw new Exception("Scene has no components of that type!");
        
        return foundComponents.ToArray() as Generic[];
    }
    
    public Generic GetComponent<Generic>() where Generic : Component {
        
        Component foundComponent = components.Find(x => x.GetType() == typeof(Generic));
        
        if(foundComponent == null)
            throw new Exception("Scene has no components of that type!");
        
        return foundComponent as Generic;
    }
    
    public void RemoveComponent(Component __component) {
        __component.End();
        components.Remove(__component);
    }
    
    public void RemoveComponents<Generic>() where Generic : Component {
        
        Component[] foundComponents = GetComponents<Generic>();
        
        if(foundComponents.Length == 0)
            throw new Exception("Scene has no components of that type!");

        foreach (Component component in foundComponents) {
            component.End();
            components.Remove(component);
            ComponentDestroyedEvent?.Invoke(this, new ComponentDestroyEvent(component, this, scene));
        }
    }
    
    public void RemoveComponent<Generic>() where Generic : Component {
        
        Component foundComponent = GetComponent<Generic>();
        
        if(foundComponent == null)
            throw new Exception("Scene has no components of that type!");
        
        foundComponent.End();
        components.Remove(foundComponent);
        ComponentDestroyedEvent?.Invoke(this ,new ComponentDestroyEvent(foundComponent, this, scene));
    }
    
    public Generic FindSingleton<Generic>() where Generic : Component
    {
        foreach (Component component in components)
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
        
        foreach (Component __component in components)
            if (__component.GetType() == typeof(Generic))
                if (__component.EnforceSingleton)
                    return true;
                else
                    return false;
        
        return false;
    }
    
    public void RecompileComponentOrder() {
        components.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        components.Reverse();
    }
}