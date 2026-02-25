using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;


namespace AwperativeKernel;




public abstract partial class Component : ComponentDocker
{
    
    
    
    /// <summary>
    /// Current parent of the Component. Can be either Scene or another Component.
    /// </summary>
    public ComponentDocker ComponentDocker { get; internal set; }



    /// <summary>
    /// If the component receives time events or not.
    /// </summary>
    public bool Enabled = true;


    
    /// <summary>
    /// Component name
    /// </summary>
    public string Name;



    ///
    internal List<Action> EventDelegates;

    
    
 




    /// <summary>
    /// Order for when Components are called on. Only applies between Components on the same Docker.
    /// </summary>
    public int Priority {
        get => _priority; set => ComponentDocker.UpdatePriority(this, value);
    } internal int _priority;
    
    
    
    
    
    /// <summary>
    /// To be called when the Component is created.
    /// </summary>
    /// <param name="__parent"> Docker that this spawned in this Component</param>
    /// <param name="__name"> Name of the component</param>
    internal void Initiate(ComponentDocker __parent, string __name, string[] __tags) {
        ComponentDocker = __parent;
        Name = __name;
        _tags = [..__tags];
        
        EventDelegates = new List<Action>(); for(int i = 0; i < Awperative.allEvents.Count; i++) EventDelegates.Add(null);
        
        
        if (Awperative._TypeAssociatedTimeEvents.TryGetValue(GetType(), out HashSet<Awperative.TimeEvent> presentEvents)) {
            foreach (Awperative.TimeEvent presentEvent in presentEvents) {
                MethodInfo info = GetType().GetMethod(presentEvent.ToString());
                Action newAction = (Action)Delegate.CreateDelegate(typeof(Action), this, info);
                EventDelegates[(int)presentEvent] = newAction;
            }
        } else {
            Debug.LogError("Awperative does not recognize the given type! Perhaps it was created after Start() was called?", ["Type"], [GetType().ToString()]);
        }
    }
    
    
    
        
    
    /// <summary>
    /// Scene the Component resides in.
    /// </summary>
    public Scene Scene => __QueryScene();
    protected Scene __QueryScene() {
        if (ComponentDocker is Scene scene) return scene;
        if (ComponentDocker is Component Component) return Component.__QueryScene();
            
        return null;
    }



    
    
    
    
    
    /// <summary>
    /// Identifiers for Components.
    /// </summary>
    public ImmutableArray<string> Tags => [.._tags];
    internal HashSet<string> _tags = [];





    /// <summary>
    /// Adds a new tag to the Component
    /// </summary>
    /// <param name="__tag"> The tag to add</param>
    public void AddTag(string __tag) => ComponentDocker.HashTaggedComponent(this, __tag);





    /// <summary>
    /// Removes a tag from the Component
    /// </summary>
    /// <param name="__tag"> The tag to remove</param>
    public void RemoveTag(string __tag) => ComponentDocker.UnhashTaggedComponent(this, __tag);
    
    

    
    
    /// <summary>
    /// All parent Dockers and the parents of the parents up until the Scene. Will only list parents of parents, not uncle dockers.
    /// </summary>
    /// <remarks> Dockers[0] is the parent of this object, and Dockers[^1] is the Scene.</remarks>
    public ImmutableArray<ComponentDocker> Dockers => __QueryDockers();
    protected ImmutableArray<ComponentDocker> __QueryDockers() {
        List<ComponentDocker> returnValue = [];
        ComponentDocker currentComponentDocker = ComponentDocker;

        while (!(currentComponentDocker is Scene)) {
            if (currentComponentDocker is Component Component) {
                returnValue.Add(currentComponentDocker);
                currentComponentDocker = Component.ComponentDocker;
            } else {
                Debug.LogError("Component has a Parent that is not a Scene or Component, Please do not use the Docker class unless you know what you are doing!", ["Component", "Type", "Docker"],
                    [GetHashCode().ToString(), GetType().ToString(), ComponentDocker.GetHashCode().ToString()]);
            }
        }

        returnValue.Add(currentComponentDocker);

        return [..returnValue];
    }
    
    
    
    

    /// <summary>
    /// Returns the Parent Component. Will be null if the Component is under a scene.
    /// </summary>
    public Component Parent => __QueryParent();
    protected Component __QueryParent() {
        if (ComponentDocker is Component Component)
            return Component;
        return null;
    }

    
    
    
    
    /// <summary>
    /// All parent Components and the parents of the parents up until the Scene. Will only list parents of parents, not uncle Components.
    /// </summary>
    public ImmutableArray<Component> Parents => __QueryComponents();
    protected ImmutableArray<Component> __QueryComponents() {
        List<Component> returnValue = [];
        ComponentDocker currentComponentDocker = ComponentDocker;
        
        while (!(currentComponentDocker is Scene))
            if (currentComponentDocker is Component Component) {
                returnValue.Add(Component); 
                currentComponentDocker = Component.ComponentDocker;
            }
        return [..returnValue];
    }
    
    
    
    /// <summary>
    /// Creates a new Scene
    /// </summary>
    /// <param name="__name">Name of the Scene</param>
    public Scene CreateScene(string __name) => Awperative.CreateScene(__name);



    /// <summary>
    /// Finds a scene.
    /// </summary>
    /// <param name="__name">Name of the Scene</param>
    /// <returns></returns>
    public Scene GetScene(string __name) => Awperative.GetScene(__name);



    /// <summary>
    /// Destroys a Scene forever
    /// </summary>
    /// <param name="__scene"> Target scene</param>
    public void RemoveScene(Scene __scene) => Awperative.CloseScene(__scene);



    /// <summary>
    /// Destroys a Scene forever
    /// </summary>
    /// <param name="__name">Name of the Scene</param>
    public void RemoveScene(string __name) => Awperative.CloseScene(__name);



    public ImmutableArray<Component> GetAllChildren() {
        List<Component> targets = [.._Components];
        for (int i = 0; i < targets.Count; i++) targets.InsertRange(i + 1, targets[i]._Components);
        return [..targets];
    }



    internal ImmutableArray<Awperative.TimeEvent> GetAllEvents() {
        if (Awperative._TypeAssociatedTimeEvents.TryGetValue(this.GetType(), out HashSet<Awperative.TimeEvent> timeEvents)) {
            return [..timeEvents];
        } else {
            Debug.LogError("Awperative doesn't recognize this type. Perhaps it was created after Start() was called?", ["Type"], [this.GetType().Name]);
            return [];
        }
    }
    
    
    internal ImmutableArray<Awperative.TimeEvent> GetAllGlobalEvents() {
        if (Awperative._TypeAssociatedTimeEvents.TryGetValue(this.GetType(), out HashSet<Awperative.TimeEvent> timeEvents)) {
            foreach (Awperative.TimeEvent timeEvent in timeEvents) if (!Awperative.globalEvents.Contains(timeEvent)) timeEvents.Remove(timeEvent);
            return [..timeEvents];
        } else {
            Debug.LogError("Awperative doesn't recognize this type. Perhaps it was created after Start() was called?", ["Type"], [this.GetType().Name]);
            return [];
        }
    }
}