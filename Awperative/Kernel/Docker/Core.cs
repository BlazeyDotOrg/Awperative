using System.Collections.Generic;
using System.Linq;


namespace Awperative;


/// <summary>
/// Base class for all Awperative entities, manages components as a requirement because that is the job of all entities. 
/// </summary>
public abstract partial class Docker
{
    internal HashSet<Behavior> _components;
}