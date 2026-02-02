using System.Linq;
using Microsoft.Xna.Framework;


namespace Awperative;


public sealed partial class Body
{
    public void Unload() { foreach (Component component in components.ToList()) component.Unload(); }
    
    public void Load() { foreach (Component component in components.ToList()) { component.Load(); } }

    public void Update(GameTime __gameTime) { foreach (Component component in components.ToList()) { component.Update(__gameTime); } }
    public void Draw(GameTime __gameTime) { foreach (Component component in components.ToList()) { component.Draw(__gameTime); } }
    
    public void Destroy() { foreach(Component component in components.ToList()) component.Destroy(); }
    public void Create() { foreach (Component component in components.ToList()) component.Create(); }
}