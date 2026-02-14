using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Awperative;

public abstract partial class Container
{
    public ImmutableArray<Component> Behaviors => [.._behaviors];
    
    
    internal HashSet<Component> _behaviors = [];
    
    
    
    /// <summary>
    /// Add a new Behavior to the Docker; This is the only way they should be created. 
    /// </summary>
    /// <param name="__args"> Arguments to construct the Behavior with</param>
    /// <typeparam name="Generic"> Type of Behavior to instantiate</typeparam>
    /// <returns></returns>
    public Component Add<Generic>(object[] __args) where Generic : Component  {
        
        //Log Action
        Debug.LogAction("Adding Component to Docker", ["Type", "Args", "Docker"], 
        [typeof(Generic).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]);
        
        
        
        //Behavior does not have a constructor that matches the given args
        if (typeof(Generic).GetConstructor((Type[])__args) == null)
        {
            Debug.LogError("Behavior cannot be constructed with the given arguments",
                ["Type", "Args"],
                [typeof(Generic).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]"]); return null;
        };
        

        Component newComponent;
        
        
        //Tries to instantiate behavior, and sends a fail call if an issue occurs.
        try { newComponent = (Generic)Activator.CreateInstance(typeof(Generic), __args); }
        catch {
            Debug.LogError("Behavior creation failed!", ["Type", "Args", "Docker"], 
                [typeof(Generic).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }

        
        //If behavior is null do not add
        if(newComponent == null) { 
            Debug.LogError("Activator created Null Behavior", ["Type", "Args", "Docker"], 
                [typeof(Generic).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }
        
        
        //Add to docker and initialize the new Behavior
        _behaviors.Add(newComponent);
        newComponent.Initiate(this);
        
        
        //Logs successful action! 
        Debug.LogState("Successfully Created Behavior and Attached it to Docker", ["Type", "Args", "Docker", "Behavior"], 
            [typeof(Generic).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString(), newComponent.GetHashCode().ToString()]);
        
        
        return newComponent;
    }
    
    
    
    /// <summary>
    /// Adds a new Behavior to the Docker; This is the only way they should be created.
    /// </summary>
    /// <typeparam name="Generic"></typeparam>
    /// <returns></returns>
    public Component Add<Generic>() where Generic : Component => Add<Generic>([]);
    


    
    
    /// <summary>
    /// Transfers a Behavior to another Docker
    /// </summary>
    /// <param name="component"> Component to move</param>
    /// <param name="__container"> Container to move component to</param>
    public void Transfer(Component __component, Container __container) {
        Debug.LogAction("Transferring Component to Different Docker", ["Component", "Type", "CurrentDocker", "NewDocker"],
            [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __container.GetHashCode().ToString()]);
        
        if (!_behaviors.Contains(__component)) { 
            Debug.LogError("Docker does not have ownership of Behavior",  ["Component", "Type", "CurrentDocker", "NewDocker"], 
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __container.GetHashCode().ToString()]); return; }
        
        
        
        //Update docker lists
        __container._behaviors.Add(__component);
        _behaviors.Remove(__component);
        
        
        
        //Update components parent
        __component.Container = __container;
        
        
        
        Debug.LogState("Successfully Transferred Component to a new Docker" , ["Component", "Type", "CurrentDocker", "NewDocker"], 
            [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __container.GetHashCode().ToString()]);
    }
    
    
    
    /// <summary>
    /// Transfers the first found Behavior of a specific type to another Docker
    /// </summary>
    /// <param name="container"> Container to move component to</param>
    /// <typeparam name="Generic"> Type of component</typeparam>
    public void Transfer<Generic>(Container container) where Generic : Component => 
        Transfer(Get<Generic>(), container);

    
    
    /// <summary>
    /// Transfers all Components in a list to another Container.
    /// </summary>
    /// <param name="__behaviors"> List of Components to transfer</param>
    /// <param name="container"> Container to move Component to</param>
    public void TransferAll(IEnumerable<Component> __behaviors, Container container) {
        foreach (Component behavior in __behaviors) Transfer(behavior, container); }
    
    
    
    /// <summary>
    /// Transfers all Components of a type to another Container.
    /// </summary>
    /// <param name="container"> Target container</param>
    /// <typeparam name="Generic"> Type of Components to transfer</typeparam>
    public void TransferAll<Generic>(Container container) where Generic : Component => TransferAll(GetAll<Generic>(), container);



    

    /// <summary>
    /// Searches and returns the first Component of a type found on the Docker.
    /// </summary>
    /// <typeparam name="Generic"> The Type of Component to search for</typeparam>
    /// <returns></returns>
    public Component Get<Generic>() where Generic : Component {
        
        Debug.LogAction("Searching for Component", ["Type", "Docker"], 
            [typeof(Generic).ToString(), GetHashCode().ToString()]);


        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in _behaviors) {
            if (component is Generic foundComponent) {
                Debug.LogState("Found Component", ["Type", "Behavior", "Docker"],
                    [typeof(Generic).ToString(), foundComponent.GetHashCode().ToString(), GetHashCode().ToString()]);
                return foundComponent;
            }
        }

        
        //Throws error if there is no Component found
        Debug.LogError("Docker does not have target Component", ["Type", "Docker"], 
            [typeof(Generic).ToString(), GetHashCode().ToString()]); return null;
    }
    
    
    
    
    
    /// <summary>
    /// Searches and returns all Components of a type found on the Docker.
    /// </summary>
    /// <typeparam name="Generic"> The Type of Components to search for</typeparam>
    /// <returns></returns>
    public ImmutableArray<Component> GetAll<Generic>() where Generic : Component {
        
        Debug.LogAction("Searching for all Components on Docker", ["Type", "Docker"], 
            [typeof(Generic).ToString(), GetHashCode().ToString()]);


        List<Component> foundComponents = [];

        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in _behaviors) {
            if (component is Generic foundComponent) {
                foundComponents.Add(foundComponent);
            }
        }

        
        //Throws error if there is no Component found
        if (foundComponents.Count == 0) {
            Debug.LogError("Docker does not have target Component", ["Type", "Docker"],
                [typeof(Generic).ToString(), GetHashCode().ToString()]);
            return [];
        }



        Debug.LogState("Found Components on Docker", ["Components", "Type", "Docker"],
            [(foundComponents.SelectMany(x => x.GetHashCode().ToString()) + "]").ToString(), typeof(Generic).ToString(),  GetHashCode().ToString()]);
        
        return foundComponents.ToImmutableArray();
    }
    
    
    
    
    
    /// <summary>
    /// Destroys a Component attached to docker
    /// </summary>
    /// <param name="component"></param>
    public void Destroy(Component component) {

        //Component is null
        if (component == null) {
            Debug.LogError("Component is null", ["CurrentDocker"],
                [GetHashCode().ToString()]); return;
        }

        
        
        //Docker doesn't have Component
        if (!_behaviors.Contains(component)) {
            Debug.LogError("Docker does not have ownership of Behavior", ["Component", "Type", "CurrentDocker"],
                [component.GetHashCode().ToString(), component.GetType().ToString(), GetHashCode().ToString()]); return;
        }
        
        


        component.Destroy();
        _behaviors.Remove(component);
    }
    
    public void Destroy<Generic>() where Generic : Component {
        try
        {
            Component foundComponent = Get<Generic>();

            foundComponent.Destroy();
            _behaviors.Remove(foundComponent);
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void DestroyAll<Generic>() where Generic : Component {
        try {
            foreach (Component component in GetAll<Generic>()) {
                component.Destroy();
                _behaviors.Remove(component);
            }
        }catch { Debug.LogError("Removal failed"); }
    }

    public void Remove(Component component)
    {
        if(!_behaviors.Contains(component)) { Debug.LogError("Body does not have a component of this type"); return; }
        
        _behaviors.Remove(component);
    }
    
    public void Remove<Generic>() where Generic : Component {
        try
        {
            Component foundComponent = Get<Generic>();

            _behaviors.Remove(foundComponent);
        }catch { Debug.LogError("Removal failed"); }
    }
    
    public void RemoveAll<Generic>() where Generic : Component {
        try {
            foreach (Component component in GetAll<Generic>()) {
                _behaviors.Remove(component);
            }
        }catch { Debug.LogError("Removal failed"); }
    }
}