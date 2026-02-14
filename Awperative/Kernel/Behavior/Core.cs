using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Awperative;



/// <summary>
/// The lowest level scripting class in Awperative. Components are scene level and provide access to all scene level methods, can be applied to any docker and inherited
/// Sadly component does not have excessive access to specific types.
/// Anything that inherits Component is built to work in any DockerEntity, which leads to generic
/// Assumptions. If you want to make a body specific or scene specific component both classes are available.
/// </summary>
public abstract partial class Behavior : Docker
{
    
    
    
    /// <summary>
    /// Current parent of the Behavior. Can be either Scene or another Behavior.
    /// </summary>
    internal Docker Docker;
    
    
    
    /// <summary>
    /// Identifiers for Behaviors.
    /// </summary>
    public List<string> Tags;
    
    
    
    /// <summary>
    /// Order for when Behaviors are called on. Only applies between Components on the same Docker.
    /// </summary>
    public int Priority;
    
    
    
    
    
    /// <summary>
    /// To be called when the Behavior is created.
    /// </summary>
    /// <param name="__parent"> Docker that this spawned in this Behavior</param>
    internal void Initiate(Docker __parent) {
        Docker = __parent;
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
    
    
}