using System;
using System.Collections.Generic;
using System.Linq;


namespace Awperative;

public sealed partial class Body 
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
    
    
    
    
    
}