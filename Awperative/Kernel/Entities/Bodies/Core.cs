using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace Awperative;

public sealed partial class Body : DockerEntity
{

    
    /// <summary>
    /// Current scene the body exists in
    /// </summary>
    public Scene Scene { get; internal set; }
    
    
    
    
    
    /// <summary>
    /// All components attached to the body
    /// </summary>
    public List<Component> Components => _components.ToList();
    internal HashSet<Component> _components { get; private set; } = [];
    
    
    
    
    
    /// <summary>
    /// All tags attached to the body
    /// </summary>
    public List<string> Tags => _tags.ToList();
    internal HashSet<string> _tags { get; private set; }= [];

    
    
    
    
    /// <summary>
    /// Position of the body
    /// </summary>
    public Transform transform { get; internal set; } = new();
    
    
    
    
    
    //Prevents outside construction
    internal Body() {}
    
    
    
    
    
    /// <summary>
    /// Creates a body in the given scene
    /// </summary>
    /// <param name="__scene"></param>
    internal Body(Scene __scene) {
        Scene = __scene;
    }

    
    
    
    
    /// <summary>
    /// Creates a body with a scene and transform
    /// </summary>
    /// <param name="__scene"></param>
    /// <param name="__transform"></param>
    internal Body(Scene __scene, Transform __transform) {
        Scene = __scene;
        transform = __transform;
    }
    
    
    
    
    
    internal void Unload() { foreach (Component component in _components) component.Unload(); }
    internal void Load() { foreach (Component component in _components) { component.Load(); } }

    
    
    internal void Update(GameTime __gameTime) { foreach (Component component in _components) { component.Update(__gameTime); } }
    internal void Draw(GameTime __gameTime) { foreach (Component component in _components) { component.Draw(__gameTime); } }
    
    
    
    internal void Destroy() { foreach(Component component in _components) component.Destroy(); }
    internal void Create() { foreach (Component component in _components) component.Create(); }
}