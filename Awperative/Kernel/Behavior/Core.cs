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

    public Scene Scene;
    public Behavior Parent = null;
    
    
    //todo tags and order
    
    
    
    internal void Initiate(Scene __scene, Docker __parent) {
        Scene = __scene;
        if (__parent is Behavior behavior) Parent = behavior;
        
        Create();
    }

    
    
    internal void End() {
        Destroy();
    }
    
    
    
    
    
    protected internal virtual void Unload() {}
    protected internal virtual void Load() {}
    
    
    
    //You know what these do
    protected internal virtual void Update(GameTime __gameTime) {}
    protected internal virtual void Draw(GameTime __gameTime) {}
    
    
    //component/body/scene is being created or destroyed
    protected internal virtual void Create() {}
    protected internal virtual void Destroy() {}
}