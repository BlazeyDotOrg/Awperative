using Microsoft.Xna.Framework;


namespace Awperative;



/// <summary>
/// The lowest level scripting class in Awperative. Components are scene level and provide access to all scene level methods, can be applied to any docker and inherited
/// Sadly component does not have excessive access to specific types.
/// Anything that inherits Component is built to work in any DockerEntity, which leads to generic
/// Assumptions. If you want to make a body specific or scene specific component both classes are available.
/// </summary>
public abstract partial class Component : DockerEntity
{
    internal DockerEntity Docker;
    
    
    
    
    
    internal virtual void Initiate(DockerEntity __docker) {
        Docker = __docker;
        Create();
    }

    
    
    internal virtual void End() {
        Destroy();
    }
    
    
    
    
    
    //GAME HAS JUST BEGUN/ended
    public virtual void Unload() {}
    
    //WE ARE LOADING STUFF
    public virtual void Load() {}
    
    //You know what these do
    public virtual void Update(GameTime __gameTime) {}
    public virtual void Draw(GameTime __gameTime) {}
    
    //component/body/scene is being created or destroyed
    public virtual void Create() {}
    public virtual void Destroy() {}
}