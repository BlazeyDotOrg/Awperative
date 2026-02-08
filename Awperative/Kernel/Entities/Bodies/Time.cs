using System.Linq;
using Microsoft.Xna.Framework;


namespace Awperative;


public sealed partial class Body
{
    internal void Unload() { foreach (Component component in _components) component.Unload(); }
    internal void Load() { foreach (Component component in _components) { component.Load(); } }

    
    
    internal void Update(GameTime __gameTime) { foreach (Component component in _components) { component.Update(__gameTime); } }
    internal void Draw(GameTime __gameTime) { foreach (Component component in _components) { component.Draw(__gameTime); } }
    
    
    
    internal void Destroy() { foreach(Component component in _components) component.Destroy(); }
    internal void Create() { foreach (Component component in _components) component.Create(); }
}