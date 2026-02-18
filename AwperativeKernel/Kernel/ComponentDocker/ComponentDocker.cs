using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;



namespace AwperativeKernel;

/// <summary>
/// Base class for all Awperative Entities. Responsible for Managing hierarchy between Components and Scenes, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component.
/// </summary>
/// <remarks> Please don't inherit this. I don't know why you would</remarks>
/// <author> Avery Norris </author>
public abstract class ComponentDocker
{
    
    
    
    /// <summary>
    /// List of all Components belonging to the Docker, Please Use Add, Get, Move and Destroy to modify it.
    /// </summary>
    public ImmutableArray<Component> Components => [.._Components];
    
    
    
    /// <summary>
    /// Amount of all Components in the Docker
    /// </summary>
    public int Count => _Components.Count;
    


    /// <summary>
    /// Core of Docker, contains all of our precious Components. Sorts them by their priorities with highest going first.
    /// If they are equal it defaults to hash codes to ensure consistent Behavior
    /// </summary>
    internal SortedSet<Component> _Components = new(_componentSorter);
    
    
    
    /// <summary>
    /// How Priority is sorted.
    /// </summary>
    private readonly static Comparer<Component> _componentSorter = Comparer<Component>.Create((a, b) => {
        int result = b.Priority.CompareTo(a.Priority);
        return (result != 0) ? result : a.GetHashCode().CompareTo(b.GetHashCode());
    });





    /// <summary>
    /// Resorts member of Component list to match the Priority.
    /// </summary>
    /// <param name="__component"> Component to modify</param>
    /// <param name="__priority"> New priority for Component</param>
    internal void UpdatePriority(Component __component, int __priority) {
        foreach (String tag in __component._tags) {
            _taggedComponents[tag].Remove(__component);
        }  _Components.Remove(__component); 
        
        __component._priority = __priority;
        
        foreach (String tag in __component._tags) {
            _taggedComponents[tag].Add(__component);
        } _Components.Add(__component);
    }
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// Called by Awperative when the game is Closed, sends the event to all children; and they send it to their children.
    /// </summary>
    /// <remarks> Will not always trigger if the program is force closed </remarks>
    internal virtual void ChainUnload() { foreach (Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Unload(); component.ChainUnload(); } } }
    
    /// <summary>
    /// Called by Awperative when the game is Opened, sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainLoad() { foreach (Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Load(); component.ChainLoad(); } } }

    
    
    /// <summary>
    /// Called by Awperative when the game is Updated sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainUpdate() { foreach (Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Update(); component.ChainUpdate(); } } }
    
    /// <summary>
    /// Called by Awperative when the game is Drawn, sends the event to all children; and they send it to their children.
    /// </summary>
    /// <remarks> Only use this method for drawing methods</remarks>
    internal virtual void ChainDraw() { foreach (Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Draw(); component.ChainDraw(); } } }
    
    
    
    /// <summary>
    /// Called by Awperative when this is Created, sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainCreate() { foreach (Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Create(); component.ChainCreate(); } } }
    
    /// <summary>
    /// Called by Awperative when this Component is destroyed, sends the event to all children; since they will be Destroyed too. And they send it to their children.
    /// </summary>
    /// <remarks> Not called when the game is closed</remarks>
    internal virtual void ChainDestroy() { foreach(Component component in (Component[])[.._Components]) { if(component.Enabled) { component.Destroy(); component.ChainDestroy(); } } }
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// Add a new Component to the Docker; This is the only way they should be created. 
    /// </summary>
    /// <param name="__args"> Arguments to construct the Component with</param>
    /// <typeparam name="__Type"> Type of Component to instantiate</typeparam>
    /// <returns></returns>
    public __Type Add<__Type>(object[] __args) where __Type : Component  {
        
        
        
        //Component does not have a constructor that matches the given args
        if (typeof(__Type).GetConstructor(__args.Select(x => x.GetType()).ToArray()) == null) {
            Debug.LogError("Component cannot be constructed with the given arguments",
                ["Type", "Args"],
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.Select(x => x.ToString())) + "]"]); return null;
        };
        

        Component newComponent;
        
        
        //Tries to instantiate Component, and sends a fail call if an issue occurs.
        try { newComponent = (__Type)Activator.CreateInstance(typeof(__Type), __args); }
        catch {
            Debug.LogError("Component creation failed!", ["Type", "Args", "Docker"], 
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.Select(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }

        
        //If Component is null do not add
        if(newComponent == null) { 
            Debug.LogError("Activator created Null Component", ["Type", "Args", "Docker"], 
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.Select(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }
        
        
        //Add to docker and initialize the new Component
        _Components.Add(newComponent);
        newComponent.Initiate(this);
        
        
        return (__Type) newComponent;
    }
    
    
    
    /// <summary>
    /// Adds a new Component to the Docker; This is the only way they should be created.
    /// </summary>
    /// <typeparam name="__Type"></typeparam>
    /// <returns></returns>
    public __Type Add<__Type>() where __Type : Component => Add<__Type>([]);



    
    
    
    
    
    
    
    /// <summary>
    /// Transfers a child Component to another Docker
    /// </summary>
    /// <param name="__component"> Component to move</param>
    /// <param name="__componentDocker"> Docker to move component to</param>
    /// <remarks> Components cannot transfer themselves with this Method!</remarks>
    public void Move(Component __component, ComponentDocker __componentDocker) {

        
        
        //This allows self transfer behavior while preserving Docker's actual job, Before all other statements to prevent Double-Debugging.
        if (__component == this) { __component.ComponentDocker.Move(__component, __componentDocker); return; }
        
        
        
        if (__component == null) {
            Debug.LogError("Component is null!",  ["CurrentDocker", "NewDocker"], 
                [GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]); return; }
        
        
        
        if (!_Components.Contains(__component)) { 
            Debug.LogError("Docker does not have ownership of Component",  ["Component", "Type", "CurrentDocker", "NewDocker"], 
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]); return; }

        

        //Update docker lists
        __componentDocker._Components.Add(__component);
        _Components.Remove(__component);
        
        
        
        //Update components parent
        __component.ComponentDocker = __componentDocker;
    }
    
    
    
    /// <summary>
    /// Transfers the first found Component of a specific type to another Docker
    /// </summary>
    /// <param name="__componentDocker"> Docker to move component to</param>
    /// <typeparam name="__Type"> Type of component</typeparam>
    public void Move<__Type>(ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(), __componentDocker);

    
    
    /// <summary>
    /// Transfers all Components in a list to another Docker.
    /// </summary>
    /// <param name="__Components"> List of Components to transfer</param>
    /// <param name="__componentDocker"> Docker to move Component to</param>
    public void MoveAll(IEnumerable<Component> __Components, ComponentDocker __componentDocker) { foreach (Component Component in (Component[])[..__Components]) Move(Component, __componentDocker); }
    
    
    
    /// <summary>
    /// Transfers all Components of a type to another Docker.
    /// </summary>
    /// <param name="__componentDocker"> Target Docker</param>
    /// <typeparam name="__Type"> Type of Components to transfer</typeparam>
    public void MoveAll<__Type>(ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(), __componentDocker);



    
    
    
    
    
    
    
    /// /// <summary>
    /// Holds Components at Keys of their tags.
    /// </summary>
    internal Dictionary<string, SortedSet<Component>> _taggedComponents = [];




    
    /// <summary>
    /// Add component to its proper place in the dictionary and resort values to match priorities.
    /// </summary>
    /// <param name="__component"> Component to hash</param>
    /// <param name="__tag"> Value to try and hash</param>
    internal void HashTaggedComponent(Component __component, string __tag) {

        if (!__component._tags.Add(__tag)) {
            Debug.LogError("Component already has tag!", ["Component", "Type", "Tag", "Docker"],
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), __tag, GetHashCode().ToString()]); return;
        }

        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components)) {
            components.Add(__component);
            
        } else { _taggedComponents.Add(__tag, new SortedSet<Component>(_componentSorter)); _taggedComponents[__tag].Add(__component); }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="__component"></param>
    /// <param name="__tag"></param>
    internal void UnhashTaggedComponent(Component __component, string __tag) {

        if (!__component._tags.Remove(__tag)) {
            Debug.LogError("Component already doesn't have that tag!", ["Component", "Type", "Tag", "Docker"],
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), __tag, GetHashCode().ToString()]); return;
        }
        
        
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components)) {
            components.Remove(__component);
            
            if(components.Count == 0)
                _taggedComponents.Remove(__tag);
        }
    }



    

    /// <summary>
    /// Finds the first instance of a component with a given tag
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    internal Component Get(string __tag) {
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components))
            return ((Component[])[..components])[0];

        return null;
    }



    /// <summary>
    /// Finds all Components with a given tag
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    internal ImmutableArray<Component> GetAll(string __tag) {
        if (_taggedComponents.TryGetValue(__tag, out SortedSet<Component> components))
            return [..components];

        return [];
    }



    /// <summary>
    /// Finds the first Component that has all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    internal Component Get(List<string> __tags) { ImmutableArray<Component> returnValue = GetAll(__tags); return returnValue.Length > 0 ? returnValue[0] : null; }



    /// <summary>
    /// Finds all Components that have all the given tags
    /// </summary>
    /// <param name="__tags"></param>
    /// <returns></returns>
    internal ImmutableArray<Component> GetAll(List<string> __tags) {

        if (__tags.Count == 0)
            return [];
        
        SortedSet<Component> foundComponents = _taggedComponents[__tags[0]];

        for (int i = 1; i < __tags.Count; i++) {
            foreach (Component component in (Component[])[..foundComponents]) {
                if (!_taggedComponents[__tags[i]].Contains(component)) foundComponents.Remove(component);
            }
        }

        return [..foundComponents];
    }
    
    
    
    

    /// <summary>
    /// Searches and returns the first Component of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Component to search for</typeparam>
    /// <returns></returns>
    public __Type Get<__Type>() where __Type : Component {
        
        
        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in (Component[])[.._Components])
            if (component is __Type foundComponent) return foundComponent;

        
        
        //Throws error if there is no Component found
        Debug.LogError("Docker does not have target Component", ["Type", "Docker"], 
            [typeof(__Type).ToString(), GetHashCode().ToString()]); return null;
    }
    
    
    
    
    
    /// <summary>
    /// Searches and returns all Components of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Components to search for</typeparam>
    /// <returns></returns>
    public ImmutableArray<__Type> GetAll<__Type>() where __Type : Component {


        
        List<__Type> foundComponents = [];

        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in (Component[])[.._Components]) {
            if (component is __Type foundComponent) {
                foundComponents.Add(foundComponent);
            }
        }

        
        //Throws error if there is no Component found
        if (foundComponents.Count == 0) {
            Debug.LogError("Docker does not have target Component", ["Type", "Docker"],
                [typeof(__Type).ToString(), GetHashCode().ToString()]);
            return [];
        }

        
        
        return [..foundComponents];
    }
    
    
    
    /// <summary>
    /// Returns a bool based on if the Docker contains a Component with the given tag or not
    /// </summary>
    /// <param name="__tag"></param>
    /// <returns></returns>
    public bool Contains(string __tag) => _taggedComponents.ContainsKey(__tag);
    
    
    
    /// <summary>
    /// Returns a bool based on if the Docker contains a Component of that type or not
    /// </summary>
    /// <typeparam name="__Type">Type of the Component</typeparam>
    /// <returns></returns>
    public bool Contains<__Type>() where __Type : Component => _Components.Any(x => x is __Type);
    
    
    
    /// <summary>
    /// Returns a bool based on if the current __Component is owned by this Docker
    /// </summary>
    /// <param name="__component"></param>
    /// <returns></returns>
    public bool Contains(Component __component) => _Components.Contains(__component);
    
    
    
    
    
    /// <summary>
    /// Destroys a Component attached to docker
    /// </summary>
    /// <param name="__component"></param>
    public void Remove(Component __component) {

        //Component is null
        if (__component == null) {
            Debug.LogError("Component is null", ["CurrentDocker"],
                [GetHashCode().ToString()]); return;
        }

        
        
        //Docker doesn't have Component
        if (!_Components.Contains(__component)) {
            Debug.LogError("Docker does not have ownership of Component", ["Component", "Type", "CurrentDocker"],
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString()]); return;
        }

        __component.Destroy();
        __component.ChainDestroy();
        foreach (string tag in __component._tags) UnhashTaggedComponent(__component, tag);
        __component.ComponentDocker = null;
        
        _Components.Remove(__component);
    }
    
    
    
    /// <summary>
    /// Destroys the first found Component of a given type
    /// </summary>
    /// <typeparam name="__Type"> Type of Component to destroy</typeparam>
    public void Remove<__Type>() where __Type : Component => Remove(Get<__Type>());



    /// <summary>
    /// Destroys all Components from a given collection.
    /// </summary>
    /// <param name="__Components"></param>
    public void RemoveAll(IEnumerable<Component> __Components) { foreach (Component component in (Component[])[..__Components]) { Remove(component); } }

    
    
    /// <summary>
    /// Destroys all Components of a given type
    /// </summary>
    /// <typeparam name="__Type"></typeparam>
    public void RemoveAll<__Type>() where __Type : Component => RemoveAll(GetAll<__Type>());



    /// <summary>
    /// Destroys all Components attached to Docker
    /// </summary>
    public void RemoveAll() { foreach (Component component in (Component[])[.._Components]) { Remove(component); } }
    
    
    
    
    
}