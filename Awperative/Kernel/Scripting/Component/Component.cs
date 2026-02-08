using Microsoft.Xna.Framework;


namespace Awperative;



/// <summary>
/// The lowest level scripting class in Awperative. Components are scene level and provide access to all scene level methods, can be applied to any docker and inherited
/// </summary>
public abstract class Component
{
    internal DockerEntity Parent;
    
    
    
    
    
    internal void Initiate(DockerEntity __parent) {
        Parent = __parent;
        Create();
    }

    
    
    internal void End() {
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