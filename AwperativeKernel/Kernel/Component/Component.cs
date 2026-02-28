using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic;


namespace AwperativeKernel;




public abstract partial class Component : ComponentDocker, IDisposable
{



    /// <summary> Current parent of the Component. Can be either Scene or another Component.</summary>
    public ComponentDocker ComponentDocker { get; internal set; } = null;


    /// <summary>
    /// Component name
    /// </summary>
    [NotNull]
    public string Name {
        get => _name;
        set { if (!NotNull.VerifyOrThrow(value)) return; _name = value; }
    } private string _name;

    
    /// <summary> Represents the state of this Component, The largest bit represents if the Component is enabled or not, while the
    /// next 7 represent its priority </summary>
    [UnsafeInternal]
    private byte OrderProfile;

    /// <summary> If the component receives time events or not. </summary>
    [CalculatedProperty] [CalculatedPropertyExpense("Very Low")]
    public bool Enabled {
        get => (OrderProfile & 128) > 0;
        set => OrderProfile = (byte)((OrderProfile & 127) | (value ? 128 : 0));
    }
    
    /// <summary> Represents the Component's Update priority, can be set to any value ranging from -64 to 63; otherwise an error will throw! </summary>
    [CalculatedProperty] [CalculatedPropertyExpense("Very Low")]
    public int Priority {
        get => (sbyte)(OrderProfile << 1) >> 1;
        set {
            if(!ValueFitsRange.VerifyOrThrow(value, -64, 63)) return;
            OrderProfile = (byte)((OrderProfile & 0x80) | (value & 0x7F));
            ComponentDocker.UpdatePriority(this, value);
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



    public void TryEvent(int __timeEvent) {
        Action<Component> eventDelegates = Awperative._TypeAssociatedTimeEvents[GetType()][__timeEvent];
        eventDelegates?.Invoke(this);
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
        List<Component> targets = [.._components];
        for (int i = 0; i < targets.Count; i++) targets.InsertRange(i + 1, targets[i]._components);
        return [..targets];
    }

    public virtual void Dispose() {
        GC.SuppressFinalize(this);
    }

    public override string ToString() {
        return this.Name;
    }
}