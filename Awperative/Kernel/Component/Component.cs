using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.Xna.Framework;


namespace Awperative;




public abstract partial class Component : ComponentDocker
{
    
    
    
    /// <summary>
    /// Current parent of the Component. Can be either Scene or another Component.
    /// </summary>
    public ComponentDocker ComponentDocker { get; internal set; }



    /// <summary>
    /// If the component receives time events or not.
    /// </summary>
    public bool Enabled;



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
    internal void Initiate(ComponentDocker __parent) {
        ComponentDocker = __parent;
        Create();
    }
    
    
    
    
    
    /// <summary>
    /// Called when the Game is Closing; does not always happen depending on if it is Force Closed.
    /// </summary>
    protected internal virtual void Unload() {}
    
    /// <summary>
    /// Called when the Game is Loading.
    /// </summary>
    protected internal virtual void Load() {}
    
    
    
    /// <summary>
    /// Called every frame before Draw, it is recommended to do any Non-Drawing update logic here.
    /// </summary>
    protected internal virtual void Update() {}
    
    /// <summary>
    /// Called after Update when the screen is being drawn. Please only put Drawing related logic here.
    /// </summary>
    protected internal virtual void Draw() {}
    
    

    /// <summary>
    /// Called when the Component is created.
    /// </summary>
    protected internal virtual void Create() {}
    
    /// <summary>
    /// Called when the Component is destroyed. Not called when the Game is closed.
    /// </summary>
    protected internal virtual void Destroy() {}
    
    
    
        
    
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
}