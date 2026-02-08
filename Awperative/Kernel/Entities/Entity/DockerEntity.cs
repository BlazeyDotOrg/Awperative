using System.Collections.Generic;
using System.Linq;


namespace Awperative;


/// <summary>
/// Base class for all Awperative entities, manages components as a requirement because that is the job of all entities. 
/// </summary>
internal abstract partial class DockerEntity
{
    public Scene scene;
    
    public List<Component> Components => _components.ToList();
    
    
    
    
    internal HashSet<Component> _components;
}