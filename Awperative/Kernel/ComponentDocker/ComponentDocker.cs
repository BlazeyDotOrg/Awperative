using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Awperative;

/// <summary>
/// Base class for all Awperative Entities. Responsible for Managing hierarchy between Components and Scenes, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component.
/// </summary>
/// <remarks> Please don't inherit this I don't know why you would</remarks>
/// <author> Avery Norris </author>
public abstract partial class ComponentDocker
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
    /// Core of Docker, contains all of our precious Components.
    /// </summary>
    internal HashSet<Component> _Components = [];
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// Called by Awperative when the game is Closed, sends the event to all children; and they send it to their children.
    /// </summary>
    /// <remarks> Will not always trigger if the program is force closed </remarks>
    internal virtual void ChainUnload() { foreach (Component component in (Component[])[.._Components]) { component.Unload(); component.ChainUnload(); } }
    
    /// <summary>
    /// Called by Awperative when the game is Opened, sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainLoad() { foreach (Component component in (Component[])[.._Components]) { component.Load(); component.ChainLoad(); } }

    
    
    /// <summary>
    /// Called by Awperative when the game is Updated sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainUpdate() { foreach (Component component in (Component[])[.._Components]) { component.Update(); component.ChainUpdate(); } }
    
    /// <summary>
    /// Called by Awperative when the game is Drawn, sends the event to all children; and they send it to their children.
    /// </summary>
    /// <remarks> Only use this method for drawing methods</remarks>
    internal virtual void ChainDraw() { foreach (Component component in (Component[])[.._Components]) { component.Draw(); component.ChainDraw(); } }
    
    
    
    /// <summary>
    /// Called by Awperative when this Component is destroyed, sends the event to all children; since they will be Destroyed too. And they send it to their children.
    /// </summary>
    /// <remarks> Not called when the game is closed</remarks>
    internal virtual void ChainDestroy() { foreach(Component component in (Component[])[.._Components]) { component.Destroy(); component.ChainDestroy(); } }
    
    /// <summary>
    /// Called by Awperative when this is Created, sends the event to all children; and they send it to their children.
    /// </summary>
    internal virtual void ChainCreate() { foreach (Component component in (Component[])[.._Components]) { component.Create(); component.ChainCreate(); } }
    
    
    
    
    
    
    
    
    
    
    /// <summary>
    /// Add a new Component to the Docker; This is the only way they should be created. 
    /// </summary>
    /// <param name="__args"> Arguments to construct the Component with</param>
    /// <typeparam name="__Type"> Type of Component to instantiate</typeparam>
    /// <returns></returns>
    public __Type Add<__Type>(object[] __args) where __Type : Component  {
        
        //Log Action
        Debug.LogAction("Adding Component to Docker", ["Type", "Args", "Docker"], 
            [typeof(__Type).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]);
        
        
        
        //Component does not have a constructor that matches the given args
        if (typeof(__Type).GetConstructor(__args.Select(x => x.GetType()).ToArray()) == null) {
            Debug.LogError("Component cannot be constructed with the given arguments",
                ["Type", "Args"],
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]"]); return null;
        };
        

        Component newComponent;
        
        
        //Tries to instantiate Component, and sends a fail call if an issue occurs.
        try { newComponent = (__Type)Activator.CreateInstance(typeof(__Type), __args); }
        catch {
            Debug.LogError("Component creation failed!", ["Type", "Args", "Docker"], 
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }

        
        //If Component is null do not add
        if(newComponent == null) { 
            Debug.LogError("Activator created Null Component", ["Type", "Args", "Docker"], 
                [typeof(__Type).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString()]); return null;
        }
        
        
        //Add to docker and initialize the new Component
        _Components.Add(newComponent);
        newComponent.Initiate(this);
        
        
        //Logs successful action! 
        Debug.LogState("Successfully Created Component and Attached it to Docker", ["Type", "Args", "Docker", "Component"], 
            [typeof(__Type).ToString(), "[" + string.Join(", ", __args.SelectMany(x => x.ToString())) + "]", GetHashCode().ToString(), newComponent.GetHashCode().ToString()]);
        
        
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


        
        Debug.LogAction("Transferring Component to Different Docker", ["Component", "Type", "CurrentDocker", "NewDocker"],
            [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]);
        
        
        
        if (__component == null) {
            Debug.LogError("Component is null!",  ["Component", "Type", "CurrentDocker", "NewDocker"], 
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]); return; }
        
        
        
        if (!_Components.Contains(__component)) { 
            Debug.LogError("Docker does not have ownership of Component",  ["Component", "Type", "CurrentDocker", "NewDocker"], 
                [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]); return; }

        

        //Update docker lists
        __componentDocker._Components.Add(__component);
        _Components.Remove(__component);
        
        
        
        //Update components parent
        __component.ComponentDocker = __componentDocker;
        
        
        
        Debug.LogState("Successfully Transferred Component to a new Docker" , ["Component", "Type", "CurrentDocker", "NewDocker"], 
            [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString(), __componentDocker.GetHashCode().ToString()]);
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
    public void MoveAll(IEnumerable<Component> __Components, ComponentDocker __componentDocker) { foreach (Component Component in (Component[])[.._Components]) Move(Component, __componentDocker); }
    
    
    
    /// <summary>
    /// Transfers all Components of a type to another Docker.
    /// </summary>
    /// <param name="__componentDocker"> Target Docker</param>
    /// <typeparam name="__Type"> Type of Components to transfer</typeparam>
    public void MoveAll<__Type>(ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(), __componentDocker);



    
    
    
    
    
    

    /// <summary>
    /// Searches and returns the first Component of a type found on the Docker.
    /// </summary>
    /// <typeparam name="__Type"> The Type of Component to search for</typeparam>
    /// <returns></returns>
    public __Type Get<__Type>() where __Type : Component {
        
        Debug.LogAction("Searching for Component", ["Type", "Docker"], 
            [typeof(__Type).ToString(), GetHashCode().ToString()]);


        
        //Iterates through the loop and returns if a match is found
        foreach (Component component in (Component[])[.._Components]) {
            if (component is __Type foundComponent) {
                Debug.LogState("Found Component", ["Type", "Component", "Docker"],
                    [typeof(__Type).ToString(), foundComponent.GetHashCode().ToString(), GetHashCode().ToString()]);
                return foundComponent;
            }
        }

        
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
        
        Debug.LogAction("Searching for all Components on Docker", ["Type", "Docker"], 
            [typeof(__Type).ToString(), GetHashCode().ToString()]);


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



        Debug.LogState("Found Components on Docker", ["Components", "Type", "Docker"],
            [(foundComponents.SelectMany(x => x.GetHashCode().ToString()) + "]").ToString(), typeof(__Type).ToString(),  GetHashCode().ToString()]);
        
        return [..foundComponents];
    }
    
    
    
    
    
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
        _Components.Remove(__component);
        
        
         
        Debug.LogState("Successfully Destroyed Component", ["Component", "Type", "CurrentDocker"],
            [__component.GetHashCode().ToString(), __component.GetType().ToString(), GetHashCode().ToString()]);
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
    public void RemoveAll(IEnumerable<Component> __Components) { foreach (Component component in (Component[])[.._Components]) { Remove(component); } }

    
    
    /// <summary>
    /// Destroys all Components of a given type
    /// </summary>
    /// <typeparam name="__Type"></typeparam>
    public void RemoveAll<__Type>() where __Type : Component => RemoveAll(GetAll<__Type>());



    /// <summary>
    /// Destroys all Components attached to Docker
    /// </summary>
    public void Clear() { foreach (Component component in (Component[])[.._Components]) { Remove(component); } }
    
    
    
    
    
}